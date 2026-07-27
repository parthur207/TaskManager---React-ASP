using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.TaskCategory;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Core.UseCases.TaskCategory
{
    public class UpdateTaskCategoryUseCase : IUpdateTaskCategoryUseCase
    {
        private readonly IUpdateTaskCategoryPort _updateTaskCategoryPort;
        private readonly ICurrentUserPort _currentUserPort;
        public UpdateTaskCategoryUseCase(IUpdateTaskCategoryPort updateTaskCategoryPort, ICurrentUserPort currentUserPort)
        {
            _updateTaskCategoryPort = updateTaskCategoryPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(UpdateTaskCategoryModel model)
        {
            var Response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Sessão expirada. Realize o login novamente.";
                Response.Status = Enums.ResponseStatusEnum.Unauthorized;
                return Response;
            }

            if (model is null)
            {
                Response.Message = "O novo nome da categoria não pode ser vazio.";
                Response.Status = ResponseStatusEnum.Error;
                return Response;
            }

            var responseRepository = await _updateTaskCategoryPort.ExecuteAsync(model);

            if (responseRepository.Status!=ResponseStatusEnum.Success)
            {
                return responseRepository;
            }

            return responseRepository;
        }
    }
}
