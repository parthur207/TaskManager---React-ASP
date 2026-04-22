using TaskManager.Adapters.DTOs;
using TaskManager.Core.Models.Task;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface ISearchTaskUseCase
    {
        Task<ResponseModel<List<TaskDTO>>> ExecuteAsync(SearchTaskModel model);
    }
}