using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class GetTaskByIdUseCase : IGetTaskByIdUseCase
    {

        private readonly IGetTaskByIdPort _getTaskByIdPort;
        public GetTaskByIdUseCase(IGetTaskByIdPort getTaskByIdPort)
        {
            _getTaskByIdPort = getTaskByIdPort;
        }
    }
}
