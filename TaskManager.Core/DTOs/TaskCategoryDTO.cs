using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Models.TaskCategory;

namespace TaskManager.Adapters.DTOs
{
    public sealed class TaskCategoryDTO
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public TaskCategoryEntity ModelToEntity(CreateTaskCategoryModel model, Guid userId)
        {
            return new TaskCategoryEntity
                (
                    userId,
                    model.Name
                );
        }
    }
}
