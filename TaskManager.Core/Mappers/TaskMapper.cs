using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Models;

namespace TaskManager.Adapters.Mappers
{
    public class TaskMapper
    {
        public static TaskEntity ModelToEntity(
            CreateTaskModel model,
            Guid userId)
        {
            return new TaskEntity(
                model.Name,
                model.Description,
                model.CategoryId,
                model.SpaceId,
                userId,
                model.Term
            );
        }

        public static TaskDTO EntityToDTO(TaskEntity entity)
        {
            return new TaskDTO
            {
                Name = entity.Name,
                Description = entity.Description ?? "",
                TaskCategoryName = entity.Category?.Name ?? "",
                SpaceName = entity.Space?.Name ?? "",
                Status = entity.StatusEnum.ToString(),
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Term = entity.Term,
                OwnerUserName = entity.OwnerUser.Name ?? entity.OwnerUser.Email.Value,
                ResponsibleUserName = entity.ResponsibleUser?.Name ?? ""
            };
        }
    }
}
