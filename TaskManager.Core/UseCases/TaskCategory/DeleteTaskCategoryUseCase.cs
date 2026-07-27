using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Core.UseCases.TaskCategory
{
    internal class DeleteTaskCategoryUseCase : IDeleteTaskCategoryUseCase
    {
        private readonly IDeleteTaskCategoryPort _deleteTaskCategoryPort;
        private readonly ICurrentUserPort _currentUserPort;
        public DeleteTaskCategoryUseCase(IDeleteTaskCategoryPort deleteTaskCategoryPort, ICurrentUserPort currentUserPort)
        {
            _deleteTaskCategoryPort = deleteTaskCategoryPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid TaskCategoryId)
        {
            var Response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Sessão expirada. Realize o login novamente.";
                Response.Status = Enums.ResponseStatusEnum.Unauthorized;
                return Response;
            }
          
            var responseRepository = await _deleteTaskCategoryPort
                .ExecuteAsync(TaskCategoryId, _currentUserPort.UserId);

            if (responseRepository.Status!=ResponseStatusEnum.Success)
            {
                return responseRepository;
            }

            return responseRepository;
        }
    }
}
