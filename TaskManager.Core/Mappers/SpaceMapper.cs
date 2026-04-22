using TaskManager.Adapters.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.Space;

namespace TaskManager.Adapters.Mappers
{
    public class SpaceMapper
    {
        public static SpaceEntity ModelToEntity(
            CreateSpaceModel model,
            Guid ownerId,
            IList<SpaceMemberEntity>? resolvedMembers = null)
        {
            return new SpaceEntity(model.Name, ownerId, resolvedMembers);
        }

        public static SpaceDTO EntityToDTO(SpaceEntity entity)
        {
            return new SpaceDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}