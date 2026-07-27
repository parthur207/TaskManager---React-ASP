using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.Core.UseCases.AI
{
    public class AI_RefineTaskAttributesUseCase : IAI_RefineTaskAttributesUseCase
    {
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;
        private readonly ICurrentUserPort _currentUserPort;
        public AI_RefineTaskAttributesUseCase(IGetTaskByIdPort getTaskByIdPort, IOllamaProviderPort ollamaProviderPort, ICurrentUserPort currentUserPort)
        {
            _getTaskByIdPort = getTaskByIdPort;
            _ollamaProviderPort = ollamaProviderPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<IEnumerable<TaskDTO>>> ExecuteAsync(Guid taskId)
        {
            var Response = new ResponseModel<IEnumerable<TaskDTO>>();
            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Sessão expirada. Realize o login novamente.";
                return Response;
            }

            throw new NotImplementedException();
        }
    }
}
