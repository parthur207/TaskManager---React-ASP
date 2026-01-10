using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    internal class DeleteTaskUseCase : IDeleteTaskUseCase
    {

        private readonly IDeleteTaskPort _deleteTaskPort;
        public DeleteTaskUseCase(IDeleteTaskPort deleteTaskPort)
        {
            _deleteTaskPort = deleteTaskPort;
        }
    }
}
