using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.TaskCategory;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Core.UseCases.TaskCategory
{
    internal class UpdateTaskCategoryUseCase : IUpdateTaskCategoryUseCase
    {

        private readonly IUpdateTaskCategoryPort _updateTaskCategoryPort;
        public UpdateTaskCategoryUseCase(IUpdateTaskCategoryPort updateTaskCategoryPort)
        {
            _updateTaskCategoryPort = updateTaskCategoryPort;
        }
    }
}
