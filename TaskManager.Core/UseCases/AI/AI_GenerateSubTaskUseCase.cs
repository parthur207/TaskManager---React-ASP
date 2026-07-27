using TaskManager.Core.Entities;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.Core.UseCases.AI
{
    public class AI_GenerateSubTaskUseCase : IAI_IGenerateSubTaskUseCase
    {
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;

        public AI_GenerateSubTaskUseCase(IGetTaskByIdPort getTaskByIdPort, IOllamaProviderPort ollamaProviderPort)
        {
            _getTaskByIdPort = getTaskByIdPort;
            _ollamaProviderPort = ollamaProviderPort;
        }

        public async Task<ResponseModel<List<TaskChildrenEntity>>> ExecuteAsync(Guid idTask)
        {
            var Response = new ResponseModel<List<TaskChildrenEntity>>();

            return Response;
        }
    }
}
