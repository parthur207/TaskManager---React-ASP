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
    public class AI_GenerateExecutionPlanUseCase : IAI_GenerateExecutionPlanUseCase
    {
        private readonly IGetTaskByIdPort _getTaskByIdPort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IOllamaProviderPort _ollamaProviderPort;
        private readonly TaskExecutionPlanPrompt _taskExecutionPlanPrompt;

        public AI_GenerateExecutionPlanUseCase(IGetTaskByIdPort getTaskByIdPort, ICurrentUserPort currentUserPort,
            IOllamaProviderPort ollamaProviderPort, TaskExecutionPlanPrompt taskExecutionPlanPrompt)
        {
            _getTaskByIdPort = getTaskByIdPort;
            _currentUserPort = currentUserPort;
            _ollamaProviderPort = ollamaProviderPort;
            _taskExecutionPlanPrompt = taskExecutionPlanPrompt;
        }

        public async Task<ResponseModel<string>> ExecuteAsync(Guid IdTask)
        {
            var Response = new ResponseModel<string>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Sessão expirada. Realize o login novamente.";
                return Response;
            }

            var ResponseRepository = await _getTaskByIdPort.ExecuteAsync(IdTask, _currentUserPort.UserId);

            if (ResponseRepository.Status != ResponseStatusEnum.Success || ResponseRepository.Content is null)
            {
                Response.Message = ResponseRepository.Message;
                Response.Status = ResponseRepository.Status;
                return Response;
            }

            var prompt = _taskExecutionPlanPrompt
                .PromptBuilder(TaskMapper
                    .EntityToDTO(ResponseRepository.Content));

            var ResponseIA = await _ollamaProviderPort.GenerateAsync<string>(prompt);

            if (ResponseIA.Status != ResponseStatusEnum.Success)
            {
                Response.Status = ResponseIA.Status;
                Response.Message = ResponseIA.Message;
                return Response;
            }

            

            Response.Content = ResponseIA.Content as string ?? string.Empty;
            Response.Status = ResponseStatusEnum.Success;
            return Response;
        }
    }
}