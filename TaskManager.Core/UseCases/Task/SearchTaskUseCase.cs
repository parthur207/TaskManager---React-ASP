using TaskManager.Adapters.DTOs;
using TaskManager.Adapters.Mappers;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class SearchTaskUseCase : ISearchTaskUseCase
    {
        private readonly ISearchTaskPort _searchTaskPort;
        private readonly ICurrentUserPort _currentUserPort;

        public SearchTaskUseCase(ISearchTaskPort searchTaskPort, ICurrentUserPort currentUserPort)
        {
            _searchTaskPort = searchTaskPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<List<TaskDTO>>> ExecuteAsync(SearchTaskModel model)
        {
            var response = new ResponseModel<List<TaskDTO>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Login expirado.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            var repositoryResponse = await _searchTaskPort.ExecuteAsync(model, _currentUserPort.UserId);

            if (repositoryResponse.Status != ResponseStatusEnum.Success)
            {
                response.Status = repositoryResponse.Status;
                response.Message = repositoryResponse.Message;
                return response;
            }

            response.Status = ResponseStatusEnum.Success;
            response.Content = repositoryResponse.Content!
                .Select(TaskMapper.EntityToDTO)
                .ToList();

            return response;
        }
    }
}