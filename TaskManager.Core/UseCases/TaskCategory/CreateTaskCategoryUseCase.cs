using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.TaskCategory;
using TaskManager.Core.Ports.User;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Core.UseCases.TaskCategory
{
    internal class CreateTaskCategoryUseCase : ICreateTaskCategoryUseCase
    {
        private readonly ICreateTaskCategoryPort _createTaskCategoryPort;
        public CreateTaskCategoryUseCase(ICreateTaskCategoryPort createTaskCategoryPort)
        {
            _createTaskCategoryPort = createTaskCategoryPort;
        }
    }
}
