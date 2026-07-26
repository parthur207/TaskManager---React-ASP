using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class DeleteTaskAdapter : IDeleteTaskPort
    {
        private readonly DbContextTaskManager _contextTask;
        private readonly ICachingPort _cachingPort;
        public DeleteTaskAdapter(DbContextTaskManager contextTask, ICachingPort cachingPort)
        {
            _contextTask = contextTask;
            _cachingPort = cachingPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(DeleteTaskModel model, Guid userId)
        {
            var Response= new SimpleResponseModel();
            try
            {
                var entity = await _contextTask.Task
                    .FirstOrDefaultAsync(x => x.Id == model.TaskId 
                    && x.SpaceId == model.SpaceId
                    && x.OwnerId == userId);
                
                if(entity is null)
                {
                    Response.Message = "Tarefa não encontrada, ou sem privilégio de exclusão.";
                    Response.Status= ResponseStatusEnum.Error;
                    return Response;
                }
                
                _contextTask.Task.Remove(entity);
                await _contextTask.SaveChangesAsync();

                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Task}_{entity.Id}");
                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Space}_{entity.SpaceId}");
                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Space_Tasks}_{entity.SpaceId}");


                Response.Message = "Tarefa excluída com sucesso.";
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro:"+ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
