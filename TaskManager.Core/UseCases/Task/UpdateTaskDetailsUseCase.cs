using TaskManager.Adapters.Mappers;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class UpdateTaskDetailsUseCase : IUpdateTaskDetailsUseCase
    {
        private readonly IUpdateTaskDetailsPort _updateTaskDetailsPort;
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly ICurrentUserPort _currentUserPort;

        public UpdateTaskDetailsUseCase(
            IUpdateTaskDetailsPort updateTaskDetailsPort,
            IGetTaskByIdPort getTaskByIdPort,
            ICurrentUserPort currentUserPort)
        {
            _updateTaskDetailsPort = updateTaskDetailsPort;
            _getTaskByIdPort = getTaskByIdPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(UpdateTaskModel model)
        {
            var response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Login expirado.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            if (model is null || model.Id == Guid.Empty)
            {
                response.Message = "Dados da tarefa inválidos.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                response.Message = "O título da tarefa não pode ser vazio.";
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            var taskResponse = await _getTaskByIdPort.ExecuteAsync(model.Id, _currentUserPort.UserId);

            if (taskResponse.Status != ResponseStatusEnum.Success)
            {
                response.Status = taskResponse.Status;
                response.Message = taskResponse.Message;
                return response;
            }

            var entity = taskResponse.Content!;

            try
            {
                entity.UpdateTitleOrDescription(model.Title);
            }
            catch (ArgumentException ex)
            {
                response.Message = ex.Message;
                response.Status = ResponseStatusEnum.Error;
                return response;
            }

            return await _updateTaskDetailsPort.ExecuteAsync(entity);
        }
    }
}