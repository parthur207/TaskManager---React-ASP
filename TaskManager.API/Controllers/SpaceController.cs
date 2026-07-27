using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Facades;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Space;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/spaces")]
    public class SpaceController : ControllerBase
    {
        private readonly SpaceUseCaseFacade _spaceUseCaseFacade;

        public SpaceController(SpaceUseCaseFacade spaceUseCaseFacade)
        {
            _spaceUseCaseFacade = spaceUseCaseFacade;
        }

        [HttpGet("sidebar-data")]
        public async Task<IActionResult> GetSpacesData()
        {
            var response = await _spaceUseCaseFacade.getSpaceDataSideBar.ExecuteAsync();

            return response.Status switch
            {
                ResponseStatusEnum.Error => BadRequest(response),
                ResponseStatusEnum.NotFound => NotFound(response),
                ResponseStatusEnum.Success => Ok(response),
                ResponseStatusEnum.Unauthorized => Unauthorized(response),
                ResponseStatusEnum.CriticalError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpaceById([FromRoute] Guid id)
        {
            var response = await _spaceUseCaseFacade.getSpaceById.ExecuteAsync(id);

            return response.Status switch
            {
                ResponseStatusEnum.Error => BadRequest(response),
                ResponseStatusEnum.NotFound => NotFound(response),
                ResponseStatusEnum.Success => Ok(response),
                ResponseStatusEnum.Unauthorized => Unauthorized(response),
                ResponseStatusEnum.CriticalError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSpace([FromBody] CreateSpaceModel model)
        {
            var createdSpace = await _spaceUseCaseFacade.create.ExecuteAsync(model);

            return createdSpace.Status switch
            {
                ResponseStatusEnum.Success => Ok(createdSpace),
                ResponseStatusEnum.Error => BadRequest(createdSpace),
                ResponseStatusEnum.CriticalError => StatusCode(500, createdSpace),
                _ => StatusCode(500, createdSpace)
            };
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSpace([FromRoute] Guid id)
        {
            var response = await _spaceUseCaseFacade.delete.ExecuteAsync(id);

            return response.Status switch
            {
                ResponseStatusEnum.Error => BadRequest(response),
                ResponseStatusEnum.NotFound => NotFound(response),
                ResponseStatusEnum.Success => Ok(response),
                ResponseStatusEnum.Unauthorized => Unauthorized(response),
                ResponseStatusEnum.CriticalError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };
        }

        [HttpPatch("leave/{id}")]
        public async Task<IActionResult> LeaveSpace([FromRoute] Guid id)
        {
            var response = await _spaceUseCaseFacade.leave.ExecuteAsync(id);

            return response.Status switch
            {
                ResponseStatusEnum.Error => BadRequest(response),
                ResponseStatusEnum.NotFound => NotFound(response),
                ResponseStatusEnum.Success => Ok(response),
                ResponseStatusEnum.Unauthorized => Unauthorized(response),
                ResponseStatusEnum.CriticalError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };
        }

        [HttpPatch("remove-member")]
        public async Task<IActionResult> RemoveMemberFromSpace([FromBody] MembersRemovedModel model)
        {
            var response = await _spaceUseCaseFacade.removeMembers.ExecuteAsync(model.SpaceId, model.MembersEmails);

            return response.Status switch
            {
                ResponseStatusEnum.Error => BadRequest(response),
                ResponseStatusEnum.NotFound => NotFound(response),
                ResponseStatusEnum.Success => Ok(response),
                ResponseStatusEnum.Unauthorized => Unauthorized(response),
                ResponseStatusEnum.CriticalError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };
        }

        [HttpPatch("add-member")]
        public async Task<IActionResult> AddMemberToSpace([FromBody] AddMembersModel model)
        {
            var response = await _spaceUseCaseFacade.addMembers.ExecuteAsync(model.spaceId, model.membersEmails);

            return response.Status switch
            {
                ResponseStatusEnum.Error => BadRequest(response),
                ResponseStatusEnum.NotFound => NotFound(response),
                ResponseStatusEnum.Success => Ok(response),
                ResponseStatusEnum.Unauthorized => Unauthorized(response),
                ResponseStatusEnum.CriticalError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateSpace([FromRoute] Guid id, [FromBody] UpdateSpaceModel model)
        {
            var response = await _spaceUseCaseFacade.updateName.ExecuteAsync(id, model);

            return response.Status switch
            {
                ResponseStatusEnum.Error => BadRequest(response),
                ResponseStatusEnum.NotFound => NotFound(response),
                ResponseStatusEnum.Success => Ok(response),
                ResponseStatusEnum.Unauthorized => Unauthorized(response),
                ResponseStatusEnum.CriticalError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };
        }

        [HttpGet("users/{spaceId}")]
        public async Task<IActionResult> GetUsersInSpace([FromRoute] Guid spaceId)
        {
            var response = await _spaceUseCaseFacade.getUsersBySpaceId.ExecuteAsync(spaceId);

            return response.Status switch
            {
                ResponseStatusEnum.Error => BadRequest(response),
                ResponseStatusEnum.NotFound => NotFound(response),
                ResponseStatusEnum.Success => Ok(response),
                ResponseStatusEnum.Unauthorized => Unauthorized(response),
                ResponseStatusEnum.CriticalError => BadRequest(response),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Erro inesperado.")
            };
        }

    }
}