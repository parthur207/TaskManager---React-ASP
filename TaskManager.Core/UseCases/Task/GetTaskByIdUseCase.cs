using TaskManager.Adapters.DTOs;
using TaskManager.Adapters.Mappers;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class GetTaskByIdUseCase : IGetTaskByIdUseCase
    {
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly ICurrentUserPort _currentUserPort;

        public GetTaskByIdUseCase(IGetTaskByIdPort getTaskByIdPort, ICurrentUserPort currentUserPort)
        {
            _getTaskByIdPort = getTaskByIdPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<TaskDTO>> ExecuteAsync(Guid taskId, Guid userId)
        {
            var response = new ResponseModel<TaskDTO>();

            if (!_currentUserPort.IsAuthenticated)
            {
                response.Message = "Login expirado.";
                response.Status = ResponseStatusEnum.Unauthorized;
                return response;
            }

            var repositoryResponse = await _getTaskByIdPort.ExecuteAsync(taskId, _currentUserPort.UserId);

            if (repositoryResponse.Status != ResponseStatusEnum.Success)
            {
                response.Status = repositoryResponse.Status;
                response.Message = repositoryResponse.Message;
                return response;
            }

            response.Status = ResponseStatusEnum.Success;
            response.Content = TaskMapper.EntityToDTO(repositoryResponse.Content!);
            return response;
        }
    }
}