using System.Text.Json.Serialization;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Ports.AI;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.Prompts;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.AI.Interfaces;

namespace TaskManager.Core.UseCases.AI
{
    public class AI_GenerateChildTaskUseCase : IAI_GenerateChildTaskUseCase
    {
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly ChildTaskPrompt _childTaskPrompt;
        public AI_GenerateChildTaskUseCase(IGetTaskByIdPort getTaskByIdPort, IOllamaProviderPort ollamaProviderPort, 
            ICurrentUserPort currentUserPort, ChildTaskPrompt childTaskPrompt)
        {
            _getTaskByIdPort = getTaskByIdPort;
            _ollamaProviderPort = ollamaProviderPort;
            _currentUserPort = currentUserPort;
            _childTaskPrompt = childTaskPrompt;
        }

        public async Task<ResponseModel<List<TaskChildrenDTO>>> ExecuteAsync(Guid idTask)
        {
            var Response = new ResponseModel<List<TaskChildrenDTO>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Sessão expirada. Realize o login novamente.";
                return Response;
            }

            var ResponseRepository = await _getTaskByIdPort.ExecuteAsync(idTask, _currentUserPort.UserId);

            if (ResponseRepository.Status!= ResponseStatusEnum.Success)
            {
                Response.Message = ResponseRepository.Message;
                Response.Status = ResponseRepository.Status;
                return Response;
            }

            var ResponseAI = await _ollamaProviderPort
                .GenerateAsync<List<TaskChildrenDTO>>(_childTaskPrompt
                    .PromptBuilder(TaskMapper.EntityToDTO(ResponseRepository.Content)));

            return ResponseAI;
        }
    }
}
