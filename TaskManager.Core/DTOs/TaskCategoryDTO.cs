using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.Models;

namespace TaskManager.Adapters.DTOs
{
    public sealed class TaskCategoryDTO
    {
        public TaskCategoryEntity ModelToEntity(CreateTaskCategoryModel model)
        {
            return new TaskCategoryEntity
                (
                    model.,
                    model.Name
                );
        }
    }
}
