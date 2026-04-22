using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    internal class DeleteTaskUseCase : IDeleteTaskUseCase
    {
        private readonly IDeleteTaskPort _deleteTaskPort;
        private readonly ICurrentUserPort _currentUserPort;

        public DeleteTaskUseCase(IDeleteTaskPort deleteTaskPort, ICurrentUserPort currentUserPort)
        {
            _deleteTaskPort = deleteTaskPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid taskId)
        {
            var response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Login expirado.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            if (taskId == Guid.Empty)
            {
                response.Message = "ID da tarefa inválido.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            var repositoryResponse = await _deleteTaskPort.ExecuteAsync(taskId, _currentUserPort.UserId);

            return repositoryResponse;
        }
    }
}