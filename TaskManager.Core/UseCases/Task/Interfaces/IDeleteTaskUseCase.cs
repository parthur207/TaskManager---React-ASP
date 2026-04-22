using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface IDeleteTaskUseCase
    {
        Task<SimpleResponseModel> ExecuteAsync(Guid taskId);
    }
}