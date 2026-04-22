using TaskManager.Adapters.DTOs;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface IGetTaskByIdUseCase
    {
        Task<ResponseModel<TaskDTO>> ExecuteAsync(Guid taskId, Guid userId);
    }
}