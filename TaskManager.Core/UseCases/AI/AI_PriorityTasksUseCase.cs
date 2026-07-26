using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.Core.UseCases.AI
{
    public class AI_PriorityTasksUseCase : IAI_PriorityTasksUseCase
    {

        private readonly IGetAllTasksBySpaceIdPort _getTaskByIdPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;

        public AI_PriorityTasksUseCase(IGetAllTasksBySpaceIdPort getAllTasksBySpaceIdPort, IOllamaProviderPort ollamaProviderPort)
        {
            _getTaskByIdPort = getAllTasksBySpaceIdPort;
            _ollamaProviderPort = ollamaProviderPort;
        }
        public async Task<ResponseModel<(string, IEnumerable<Guid>)>> ExecuteAsync(Guid spaceId)
        {
            throw new NotImplementedException();
        }
    }
}
