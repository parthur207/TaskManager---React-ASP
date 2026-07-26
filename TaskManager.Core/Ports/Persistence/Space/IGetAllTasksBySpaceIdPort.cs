using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Core.Ports.Persistence.Space
{
    public interface IGetAllTasksBySpaceIdPort
    {
        Task<ResponseModel<IEnumerable<TaskEntity>>> ExecuteAsync(Guid spaceId);
    }
}
