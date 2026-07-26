using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.UseCases.AI.Interfaces
{
    public interface IAI_RefineTaskAttributesUseCase
    {
        Task<ResponseModel<IEnumerable<TaskDTO>>> ExecuteAsync(Guid idTask);
    }
}

