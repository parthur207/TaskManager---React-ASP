using TaskManager.Core.Enums;
using TaskManager.Core.Models.User;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.User.Interfaces;
using TaskManager.Core.ValueObjects;

namespace TaskManager.Core.UseCases.User
{
    public class UpdateUserPasswordUseCase : IUpdateUserPasswordUseCase
    {
        private readonly IUpdateUserPasswordPort _updateUserPasswordPort;
        private readonly ICurrentUserPort _currentUserPort;

        public UpdateUserPasswordUseCase(
            IUpdateUserPasswordPort updateUserPasswordPort,
            ICurrentUserPort currentUserPort)
        {
            _updateUserPasswordPort = updateUserPasswordPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(UpdateUserPasswordModel model)
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
                response.Message = "Os dados de atualização de senha não podem ser nulos.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            if (string.IsNullOrWhiteSpace(model.OldPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
            {
                response.Message = "A senha antiga e a nova senha são obrigatórias.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            if (model.OldPassword.Equals(model.NewPassword))
            {
                response.Message = "A nova senha deve ser diferente da senha atual informada.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            var newPassword = new PasswordVO(model.NewPassword, false, true);
            model.NewPassword = newPassword.Value;

            return await _updateUserPasswordPort.ExecuteAsync(_currentUserPort.UserId, model);
        }
    }
}