using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.Ports.Persistence.Task
{
    public interface IAssignChildTaskPort
    {
        Task<SimpleResponseModel> AssignChildTaskAsync(Guid TaskParent, Guid userId, TaskChildrenEntity entity);
    }
}
