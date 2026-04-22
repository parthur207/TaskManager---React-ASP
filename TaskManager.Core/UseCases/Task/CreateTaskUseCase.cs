using TaskManager.Adapters.Mappers;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class CreateTaskUseCase : ICreateTaskUseCase
    {
        private readonly ICreateTaskPort _createTaskPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IUserQueryPort _userQuery;
        private readonly ISpaceMembershipQueryPort _membership;

        public CreateTaskUseCase(
            ICreateTaskPort createTaskPort,
            IUserQueryPort userQuery,
            ISpaceMembershipQueryPort membership,
            ICurrentUserPort currentUserPort)
        {
            _createTaskPort = createTaskPort;
            _userQuery = userQuery;
            _membership = membership;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskModel model)
        {
            var response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Login expirado.";
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

            // Verifica se o usuário atual é membro do space informado
            var membershipResponse = await _membership.IsUserMemberAsync(_currentUserPort.UserId, model.SpaceId);
            if (membershipResponse.Status != ResponseStatusEnum.Success || !membershipResponse.Content)
            {
                response.Message = "Você não tem acesso a este espaço.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            Guid responsibleUserId = _currentUserPort.UserId;

            // Se foi informado um responsável diferente, valida se ele existe e é membro do space
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

            return await _createTaskPort.ExecuteAsync(entity);
        }
    }
}