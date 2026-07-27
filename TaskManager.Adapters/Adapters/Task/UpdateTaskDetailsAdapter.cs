using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.Task;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class UpdateTaskDetailsAdapter : IUpdateTaskDetailsPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;
        public UpdateTaskDetailsAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid userId, TaskEntity entity)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (entity is null)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Entidade inválida ou nula.";
                    return Response;
                }

                var existing = await _context.Task.FirstOrDefaultAsync(t => t.Id == entity.Id);
                if (existing is null)
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "Tarefa não encontrada.";
                    return Response;
                }

                existing.UpdateTitleOrDescription(entity.Title, entity.Description);
                if (!existing.StatusEnum.Equals(entity.StatusEnum))
                {
                    existing.UpdateStatusTask(entity.StatusEnum);
                }

                if (entity.ResponsibleUserId != null && entity.ResponsibleUserId != existing.ResponsibleUserId)
                {
                    existing.AssignResponsibleUser(entity.ResponsibleUserId.Value);
                }

                _context.Task.Update(existing);
                await _context.SaveChangesAsync();

                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Task}_{entity.Id}");
                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Spaces_User}{entity.SpaceId}");
                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Space}_{entity.SpaceId}");

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Tarefa atualizada com sucesso.";
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro:" + ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
