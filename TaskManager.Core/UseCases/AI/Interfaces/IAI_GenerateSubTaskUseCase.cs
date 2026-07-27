using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.AI.Interfaces
{
    public interface IAI_GenerateSubTaskUseCase
    {
        Task<ResponseModel<List<TaskChildrenEntity>>> ExecuteAsync(Guid idTask);
    }
}
