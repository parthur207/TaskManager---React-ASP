using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class DeleteTaskUseCase : IDeleteTaskUseCase
    {
        private readonly IDeleteTaskPort _deleteTaskPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly ISignalRNotifier _notifier;

        public DeleteTaskUseCase(IDeleteTaskPort deleteTaskPort, ICurrentUserPort currentUserPort, ISignalRNotifier notifier)
        {
            _deleteTaskPort = deleteTaskPort;
            _currentUserPort = currentUserPort;
            _notifier = notifier;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(DeleteTaskModel model)
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
                response.Message = "Erro. Dados nulos ou inválidos.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            var repositoryResponse = await _deleteTaskPort.ExecuteAsync(model, _currentUserPort.UserId);
            
            if(repositoryResponse.Status == ResponseStatusEnum.Success)
                await _notifier.NotifyTaskDeleted(model.SpaceId, model.TaskId);

            return repositoryResponse;
        }
    }
}