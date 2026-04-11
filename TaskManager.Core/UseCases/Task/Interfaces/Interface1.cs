using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.DTOs;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.UseCases.Task.Interfaces
{
    public interface IGetTasksBySpaceUseCase
    {
        Task<ResponseModel<IEnumerable<TaskDTO>>> ExecuteAsync();
    }
}
