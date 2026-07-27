using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class CreateTaskUseCase : ICreateTaskUseCase
    {
        private readonly ICreateTaskPort _createTaskPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IUserQueryPort _userQuery;
        private readonly ISpaceMembershipQueryPort _membership;
        private readonly ISignalRNotifier _notifier;

        public CreateTaskUseCase(
            ICreateTaskPort createTaskPort,
            IUserQueryPort userQuery,
            ISpaceMembershipQueryPort membership,
            ICurrentUserPort currentUserPort,
            ISignalRNotifier notifier)
        {
            _createTaskPort = createTaskPort;
            _userQuery = userQuery;
            _membership = membership;
            _currentUserPort = currentUserPort;
            _notifier = notifier;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskModel model)
        {
            var response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Sessão expirada. Realize o login novamente.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            if (model is null)
            {
                response.Message = "Dados da tarefa não podem ser nulos.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                response.Message = "O nome da tarefa é obrigatório.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            var membershipResponse = await _membership.IsUserMemberAsync(_currentUserPort.UserId, model.SpaceId);
            if (membershipResponse.Status != ResponseStatusEnum.Success || !membershipResponse.Content)
            {
                response.Message = "Você não tem acesso a este espaço.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            Guid responsibleUserId = _currentUserPort.UserId;

            if (!string.IsNullOrWhiteSpace(model.ResponsibleEmail))
            {
                var userResponse = await _userQuery.GetUserByEmailAsync(model.ResponsibleEmail);
                if (userResponse.Status != ResponseStatusEnum.Success || userResponse.Content is null)
                {
                    response.Message = "O usuário responsável informado não foi encontrado.";
                    response.Status = ResponseStatusEnum.Error;
                    return response;
                }

                var responsibleMembership = await _membership.IsUserMemberAsync(userResponse.Content.Id, model.SpaceId);
                if (responsibleMembership.Status != ResponseStatusEnum.Success || !responsibleMembership.Content)
                {
                    response.Message = "O usuário responsável não é membro deste espaço.";
                    response.Status = ResponseStatusEnum.Error;
                    return response;
                }

                responsibleUserId = userResponse.Content.Id;
            }

            var entity = TaskMapper.ModelToEntity(model, _currentUserPort.UserId, responsibleUserId);

            var responseRepository= await _createTaskPort.ExecuteAsync(entity);

            if(responseRepository.Status == ResponseStatusEnum.Success)
                await _notifier.NotifyTaskCreated(model.SpaceId, TaskMapper.EntityToDTO(entity));

            return responseRepository;
        }
    }
}