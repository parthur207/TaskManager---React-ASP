using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.Ports.Persistence.Task
{
    public interface IAssignExecutionPlanTaskPort
    {
        Task<SimpleResponseModel> ExecuteAsync(string? executionPlan);
    }
}
