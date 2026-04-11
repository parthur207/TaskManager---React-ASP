using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Mappers;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.Task
{
    public class DeleteTaskAdapter : IDeleteTaskPort
    {
        private readonly DbContextTaskManager _contextTask;
        public DeleteTaskAdapter(DbContextTaskManager contextTask)
        {
            _contextTask = contextTask;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid IdTask, Guid IdUser)
        {
            var Response= new SimpleResponseModel();
            try
            {
                var entity = await _contextTask.Task
                    .FirstOrDefaultAsync(x => x.Id == IdUser 
                    && x.OwnerId == IdUser);
                
                if(entity is null)
                {
                    Response.Message = "Tarefa não encontrada, ou sem privilégio de exclusão.";
                    Response.Status= ResponseStatusEnum.Error;
                    return Response;
                }

               await _contextTask.Task.ExecuteDeleteAsync();



            }
            catch (Exception ex)
            {

            }

            throw new NotImplementedException();
        }
    }
}
