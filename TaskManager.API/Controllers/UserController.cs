using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.User;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserUseCaseFacade _userUseCaseFacade;
        public UserController(UserUseCaseFacade userUseCaseFacade)
        {
            _userUseCaseFacade = userUseCaseFacade;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromForm] LoginRequestModel model)
        {
            var Response = await _userUseCaseFacade.Login.ExecuteAsync(model);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] CreateUserModel model)
        {
            var Response = await _userUseCaseFacade.Create.ExecuteAsync(model);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [Authorize]
        [HttpPost("update-password")]
        public async Task<ActionResult> UpdatePassword([FromForm] UpdateUserPasswordModel model)
        {
            var Response = await _userUseCaseFacade.UpdatePassword.ExecuteAsync(model);

            switch (Response.Status)
            {
                case ResponseStatusEnum.Error:
                    return BadRequest(Response);

                case ResponseStatusEnum.NotFound:
                    return NotFound(Response);

                case ResponseStatusEnum.Success:
                    return Ok(Response);

                case ResponseStatusEnum.CriticalError:
                    return BadRequest(Response);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.");
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetDataProfile()
        {
            var Response = await _userUseCaseFacade.GetDatasProfile.ExecuteAsync();

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

        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser()
        {
            var Response = await _userUseCaseFacade.Delete.ExecuteAsync();
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
