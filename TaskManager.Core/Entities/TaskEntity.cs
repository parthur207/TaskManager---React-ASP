using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities
{
    public sealed class TaskEntity
    {
        public TaskEntity(string name, string? description, Guid categoryId, Guid userId, DateOnly term)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CategoryId = categoryId;
            UserId = userId;
            StatusEnum = TaskStatusEnum.NotStarted;
            Term = term;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; private set; }
        public Guid CategoryId { get; private set; }
        public TaskCategoryEntity? Category { get; private set; }
        public Guid SpaceId { get; private set; }
        public SpaceEntity Space { get;private set; }
        public TaskStatusEnum StatusEnum { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateOnly Term { get; private set; }

        public void UpdateNameOrDescription(string newName = null, string newDescription = null)
        {
            if (string.IsNullOrEmpty(newName) &&
                string.IsNullOrEmpty(Description))
            {
                throw new ArgumentNullException("Erro. Nome e descrição nulos.");
            }
            Name = newName;
            Description = newDescription;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateStatusTask(TaskStatusEnum newStatus)
        {
            if (StatusEnum.Equals(newStatus))
            {
                throw new ArgumentException($"O status '{newStatus.ToString()}' já se encontra definido.");
            }

            if (StatusEnum != TaskStatusEnum.NotStarted
               && newStatus is TaskStatusEnum.NotStarted)
            {
                throw new ArgumentException($"Não é possível atribuir o status de '{newStatus.ToString()}', pois a tarefa se encontra com.");
            }
            StatusEnum = newStatus;
        }
    }
}
