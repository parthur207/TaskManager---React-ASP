using TaskManager.Core.DTOs;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.Core.UseCases.AI
{
    public class AI_RefineTaskAttributesUseCase : IAI_RefineTaskAttributesUseCase
    {
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;

        public AI_RefineTaskAttributesUseCase(IGetTaskByIdPort getTaskByIdPort, IOllamaProviderPort ollamaProviderPort)
        {
            _getTaskByIdPort = getTaskByIdPort;
            _ollamaProviderPort = ollamaProviderPort;
        }

        public Task<ResponseModel<IEnumerable<TaskDTO>>> ExecuteAsync(Guid taskId)
        {
            throw new NotImplementedException();
        }
    }
}
