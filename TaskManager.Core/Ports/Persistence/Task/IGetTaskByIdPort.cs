using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.Ports.Persistence.Task
{
    public interface IGetTaskByIdPort
    {
        Task<ResponseModel<TaskEntity>> ExecuteAsync(Guid TaskId, Guid UserId);
    }
}
