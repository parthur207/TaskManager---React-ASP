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
    internal class DeleteTaskCategoryUseCase : IDeleteTaskCategoryUseCase
    {
        private readonly IDeleteTaskCategoryPort _deleteTaskCategoryPort;
        public DeleteTaskCategoryUseCase(IDeleteTaskCategoryPort deleteTaskCategoryPort)
        {
            _deleteTaskCategoryPort = deleteTaskCategoryPort;
        }
    }
}
