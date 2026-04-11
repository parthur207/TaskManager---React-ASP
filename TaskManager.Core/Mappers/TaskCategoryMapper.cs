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
    public class TaskCategoryMapper
    {
        public static TaskCategoryDTO EntityToDTO(TaskCategoryEntity entity)
        {
            return new TaskCategoryDTO()
            {

            };
        }

        public static TaskCategoryEntity ModelToEntity(CreateTaskCategoryModel model)
        {
            return new TaskCategoryEntity
                (
                
                );
        }
    }
}
