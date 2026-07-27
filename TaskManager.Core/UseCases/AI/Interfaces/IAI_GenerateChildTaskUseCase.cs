using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.AI.Interfaces
{
    public interface IAI_GenerateChildTaskUseCase
    {
        Task<ResponseModel<List<TaskChildrenDTO>>> ExecuteAsync(Guid idTask);
    }
}
