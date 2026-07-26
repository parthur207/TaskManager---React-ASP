using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.AI;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/ai/tasks")]
    public class AIController : ControllerBase
    {
        public readonly IOllamaProviderPort _ollamaProviderPort;
        public readonly AiUseCaseCaseFacade _aiUseCaseCaseFacade;
        public AIController(IOllamaProviderPort ollamaProviderPort, AiUseCaseCaseFacade aiUseCaseCaseFacade = null)
        {
            _ollamaProviderPort = ollamaProviderPort;
            _aiUseCaseCaseFacade = aiUseCaseCaseFacade;
        }

        [HttpPost("analyze/{id}")]
        public async Task<ActionResult> GenerateRefineTaskAttributes([FromRoute] Guid idTask)
        {
            var Response = await _aiUseCaseCaseFacade.aI_RefineTaskAttributesUseCase.ExecuteAsync(idTask);
            
            switch (Response.Status)
            {
                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }     
        }

        [HttpPost("subTask/{idTask}")]
        public async Task<ActionResult> GenerateSubTask([FromRoute] Guid idTask)
        {
            var Response = await _aiUseCaseCaseFacade.aI_IGenerateSubTaskUseCase.ExecuteAsync(idTask);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [HttpPost("priority/{spaceId}")]
        public async Task<ActionResult> GeneratePriority([FromRoute] Guid spaceId)
        {
            var Response = await _aiUseCaseCaseFacade.aI_PriorityTasksUseCase.ExecuteAsync(spaceId);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [HttpPost("execution-plan/{idTask}")]
        public async Task<ActionResult> GenerateExecutionPlan([FromRoute] Guid idTask)
        {
            var Response = await _aiUseCaseCaseFacade.aI_GenerateExecutionPlanUseCase.ExecuteAsync(idTask);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }
    }
}