using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
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

        public async Task<ResponseModel<string>> ExecuteAsync(Guid IdTask, Guid SpaceId)
        {
            var Response= new ResponseModel<string>();

            var ResponseRepository = await _getTaskByIdPort.ExecuteAsync(IdTask, SpaceId, _currentUserPort.UserId);

            if (ResponseRepository.Status != ResponseStatusEnum.Success)
            {
                Response.Message = ResponseRepository.Message;
                Response.Status = ResponseRepository.Status;
                return Response;
            }

            var ResponseIA = await _ollamaProviderPort
                .GenerateAsync(_taskExecutionPlanPrompt
                .PromptBuilder(TaskMapper.EntityToDTO(ResponseRepository.Content)));
            Response.Content = ResponseIA.Content;
            Response.Status=ResponseStatusEnum.Success;
            return Response;

        }
    }
}
