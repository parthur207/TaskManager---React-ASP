using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Core.UseCases.TaskCategory
{
    public class GetAllTaskCategoryUseCase : IGetAllTaskCategoryUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IGetAllTaskCategoryPort _taskCategoryRepositoryPort;
        public GetAllTaskCategoryUseCase(ICurrentUserPort currentUserPort, IGetAllTaskCategoryPort taskCategoryRepositoryPort)
        {
            _currentUserPort = currentUserPort;
            _taskCategoryRepositoryPort = taskCategoryRepositoryPort;
        }

        public async Task<ResponseModel<IEnumerable<TaskCategoryDTO>>> ExecuteAsync(Guid spaceId)
        {
            var Response = new ResponseModel<IEnumerable<TaskCategoryDTO>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Sessão expirada. Realize o login novamente.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }

            var ResponseRepository = await _taskCategoryRepositoryPort
                .ExecuteAsync(spaceId, _currentUserPort.UserId);

            Response.Content = TaskCategoryMapper.ListEntityToListDTO(ResponseRepository?.Content);
            Response.Message = ResponseRepository.Message;
            Response.Status = ResponseRepository.Status;
            return Response;
        }
    }
}
