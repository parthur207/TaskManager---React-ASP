using TaskManager.Core.UseCases.Task.Interfaces;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.API.Facades
{
    public class TaskUseCaseFacade
    {
        public ICreateTaskUseCase Create { get; }
        public IUpdateTaskDetailsUseCase Update { get; }
        public IDeleteTaskUseCase Delete { get; }
        public IGetTaskByIdUseCase GetById { get; }
        public ISearchTaskUseCase Search { get; }

        public TaskUseCaseFacade(
            ICreateTaskUseCase create,
            IUpdateTaskDetailsUseCase update,
            IDeleteTaskUseCase delete,
            IGetTaskByIdUseCase getById,
            ISearchTaskUseCase search)
        {
            Create = create;
            Update = update;
            Delete = delete;
            GetById = getById;
            Search = search;
        }
    }
}
