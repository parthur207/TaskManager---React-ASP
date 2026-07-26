using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/task")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly TaskUseCaseFacade _taskUseCaseFacade;
        public TasksController(ILogger<TasksController> logger, TaskUseCaseFacade taskUseCaseFacade)
        {
            _logger = logger;
            _taskUseCaseFacade = taskUseCaseFacade;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel model)
        {
            var Response = await _taskUseCaseFacade.Create.ExecuteAsync(model);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);

                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskModel model)
        {
            var Response = await _taskUseCaseFacade.Update.ExecuteAsync(model);
            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);
                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);
                case ResponseStatusEnum.Success:
                    return Ok(Response);
                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);
                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTask([FromQuery] DeleteTaskModel model)
        {
            var Response = await _taskUseCaseFacade.Delete.ExecuteAsync(model);
            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);
                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);
                case ResponseStatusEnum.Success:
                    return Ok(Response);
                case ResponseStatusEnum.Unauthorized:
                    return Unauthorized(Response);
                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }
    }
}

