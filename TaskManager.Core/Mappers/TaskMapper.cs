using TaskManager.Adapters.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.Task;

namespace TaskManager.Adapters.Mappers
{
    public class TaskMapper
    {
        public static TaskEntity ModelToEntity(
            CreateTaskModel model,
            Guid ownerId,
            Guid responsibleUserId)
        {
            return new TaskEntity(
                model.Name,
                model.Description,
                model.CategoryId,
                model.SpaceId,
                ownerId,
                responsibleUserId,
                model.Term
            );
        }

        public static TaskDTO EntityToDTO(TaskEntity entity)
        {
            return new TaskDTO
            {
                Id = entity.Id,
                Name = entity.Title,
                Description = entity.Description ?? string.Empty,
                TaskCategoryName = entity.Category?.Name ?? string.Empty,
                SpaceName = entity.Space?.Name ?? string.Empty,
                Status = entity.StatusEnum.ToString(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Term = entity.Term,
                OwnerUserName = entity.OwnerUser?.Name ?? entity.OwnerUser?.Email.Value ?? string.Empty,
                ResponsibleUserName = entity.ResponsibleUser?.Name ?? string.Empty
            };
        }
    }
}