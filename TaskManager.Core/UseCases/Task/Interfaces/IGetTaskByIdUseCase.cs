using TaskManager.Core.DTOs;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface IGetTaskByIdUseCase
    {
        Task<ResponseModel<TaskDTO>> ExecuteAsync(Guid taskId);
    }
}