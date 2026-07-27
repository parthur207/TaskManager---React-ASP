using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskManager.Core.Enums;

namespace TaskManager.Core.Entities
{
    public sealed class TaskEntity
    {
        public TaskEntity() { }

        public TaskEntity(string title, string? description, 
            Guid? categoryId, Guid spaceId, Guid ownerId, Guid? responsibleUserId, DateOnly term)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            CategoryId = categoryId;
            SpaceId = spaceId;
            OwnerId = ownerId;
            ResponsibleUserId = responsibleUserId;
            ChildTasks = [];
            StatusEnum = TaskStatusEnum.NotStarted;
            Term = term;
            CreatedAt = DateTime.UtcNow;
            PrioritySort = 0;
        }

        [JsonInclude]
        public Guid Id { get; private set; }

        [JsonInclude]
        public string Title { get; private set; }

        [JsonInclude]
        public string? Description { get; private set; }

        [JsonInclude]
        public Guid OwnerId { get; set; }

        [JsonInclude]
        public UserEntity OwnerUser { get; private set; }

        [JsonInclude]
        public Guid? ResponsibleUserId { get; private set; }

        [JsonInclude]
        public UserEntity? ResponsibleUser { get; set; }

        [JsonInclude]
        public Guid? CategoryId { get; private set; }

        [JsonInclude]
        public TaskCategoryEntity? Category { get; private set; }

        [JsonInclude]
        public Guid SpaceId { get; private set; }

        [JsonInclude]
        public SpaceEntity Space { get;private set; }

        [JsonInclude]
        public IList<TaskChildrenEntity>? ChildTasks { get; private set; }

        [JsonInclude]
        public TaskStatusEnum StatusEnum { get; private set; }

        [JsonInclude]
        public DateTime CreatedAt { get; private set; }

        [JsonInclude]
        public DateTime? UpdatedAt { get; private set; }

        [JsonInclude]
        public DateOnly Term { get; private set; }

        [JsonInclude]
        public int PrioritySort { get; private set; }

        public void UpdateTitleOrDescription(string newTitle = null, string newDescription = null)
        {
            if (string.IsNullOrEmpty(newTitle) && string.IsNullOrEmpty(newDescription))
                throw new ArgumentNullException("Erro. Título e descrição não podem ser ambos vazios.");

            if (!string.IsNullOrEmpty(newTitle))
            {
                Title = newTitle;
                UpdatedAt = DateTime.UtcNow;
            }

            if (!string.IsNullOrEmpty(newDescription))
            {
                Description = newDescription;
                UpdatedAt = DateTime.UtcNow;
            }
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
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateTerm(DateOnly newTerm)
        {
            if (Term.Equals(newTerm))
            {
                throw new ArgumentException($"O prazo '{newTerm.ToString()}' já se encontra definido.");
            }
            Term = newTerm;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AssignResponsibleUser(Guid responsibleUserId)
        {
            if (ResponsibleUserId.Equals(responsibleUserId))
            {
                throw new ArgumentException("O usuário já é responsável por esta tarefa.");
            }
            ResponsibleUserId = responsibleUserId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePrioritySort(int newPrioritySort)
        {
            if (PrioritySort.Equals(newPrioritySort))
            {
                throw new ArgumentException($"A prioridade '{newPrioritySort.ToString()}' já se encontra definida.");
            }
            PrioritySort = newPrioritySort;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
