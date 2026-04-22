using TaskManager.Adapters.Mappers;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Space;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class CreateSpaceUseCase : ICreateSpaceUseCase
    {
        private readonly ICreateSpacePort _createSpacePort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IUserQueryPort _userQuery;

        public CreateSpaceUseCase(
            ICreateSpacePort createSpacePort,
            ICurrentUserPort currentUserPort,
            IUserQueryPort userQuery)
        {
            _createSpacePort = createSpacePort;
            _currentUserPort = currentUserPort;
            _userQuery = userQuery;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateSpaceModel model)
        {
            var response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Login expirado.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            if (model is null || string.IsNullOrWhiteSpace(model.Name))
            {
                response.Message = "O nome do espaço é obrigatório.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            var memberIds = new List<Core.Entities.SpaceMemberEntity>();

            if (model.MembersEmails is not null && model.MembersEmails.Any())
            {
                foreach (var email in model.MembersEmails.Distinct())
                {
                    // Não adiciona o próprio criador como membro duplicado
                    if (email.Equals(_currentUserPort.Email, StringComparison.OrdinalIgnoreCase))
                        continue;

                    var userResponse = await _userQuery.GetUserByEmailAsync(email);
                    if (userResponse.Status != ResponseStatusEnum.Success || userResponse.Content is null)
                    {
                        response.Message = $"Usuário com e-mail '{email}' não encontrado.";
                        response.Status = ResponseStatusEnum.NotFound;
                        return response;
                    }

                    memberIds.Add(new Core.Entities.SpaceMemberEntity(Guid.Empty, userResponse.Content.Id));
                }
            }

            var entity = SpaceMapper.ModelToEntity(model, _currentUserPort.UserId, memberIds);

            return await _createSpacePort.ExecuteAsync(entity, _currentUserPort.UserId);
        }
    }
}