using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.DTOs;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class GetTasksBySpace : IGetTasksBySpaceUseCase
    {
        private readonly 
        public Task<ResponseModel<IEnumerable<TaskDTO>>> ExecuteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
