using TaskManager.Core.Enums;
using TaskManager.Core.Models.TaskCategory;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Core.UseCases.TaskCategory
{
    internal class CreateTaskCategoryUseCase : ICreateTaskCategoryUseCase
    {
        private readonly ICreateTaskCategoryPort _createTaskCategoryPort;
        private readonly ICurrentUserPort _currentUserPort;
        public CreateTaskCategoryUseCase(ICreateTaskCategoryPort createTaskCategoryPort, ICurrentUserPort currentUserPort)
        {
            _createTaskCategoryPort = createTaskCategoryPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskCategoryModel model)
        {
            var Response = new SimpleResponseModel();


            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Sessão expirada. Realize o login novamente.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }


            if (model == null)
            {
                Response.Status=ResponseStatusEnum.Error;
                Response.Message = "O modelo não pode ser nulo";
                return Response;
            }

            var responseRepository= await _createTaskCategoryPort.ExecuteAsync(model, _currentUserPort.UserId);

            if (responseRepository.Status!=ResponseStatusEnum.Success)
            {
                return responseRepository;
            }

            return responseRepository;
        }
    }
}
