using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.Ports.TaskCategory;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    internal class CreateTaskUseCase : ICreateTaskUseCase
    {

        private readonly ICreateTaskPort _createTaskPort;
        public CreateTaskUseCase(ICreateTaskPort createTaskPort)
        {
            _createTaskPort = createTaskPort;
        }
    }
}
