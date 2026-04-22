using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.UseCases.User
{
    internal class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly IDeleteUserPort _deleteUserPort;
        private readonly ICurrentUserPort _currentUserPort;

        public DeleteUserUseCase(IDeleteUserPort deleteUserPort, ICurrentUserPort currentUserPort)
        {
            _deleteUserPort = deleteUserPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync()
        {
            var response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Login expirado.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            return await _deleteUserPort.ExecuteAsync(_currentUserPort.UserId);
        }
    }
}