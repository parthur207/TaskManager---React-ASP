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
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Adapters.Adapters.Task
{
    public class GetTaskByIdAdapter : IGetTaskByIdPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;
        private readonly ISpaceMembershipQueryPort _spaceMembershipQueryPort;
        public GetTaskByIdAdapter(DbContextTaskManager context, ICachingPort cachingPort, ISpaceMembershipQueryPort spaceMembershipQueryPort)
        {
            _context = context;
            _cachingPort = cachingPort;
            _spaceMembershipQueryPort = spaceMembershipQueryPort;
        }

        public async Task<ResponseModel<TaskEntity>> ExecuteAsync(Guid TaskId, Guid SpaceId, Guid UserId)
        {
            var Response = new ResponseModel<TaskEntity>();
            try
            {
                var isUserMember = await _spaceMembershipQueryPort.IsUserMemberAsync(UserId, SpaceId);
                if (!isUserMember.Content)
                {
                    Response.Message=isUserMember.Message;
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    return Response;
                }

                if (!await _context.Space.AnyAsync(x=>x.Tasks.Any(y=>y.Id==TaskId)))
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "Erro. Tarefa não encontrada no espaço informado.";
                    return Response;
                }

                var responseCache = await _cachingPort
                    .GetAsync<TaskEntity?>($"{KeysCachingEnum.Task}_{TaskId}");
                
                if (responseCache != null)
                {
                    Response.Content = responseCache;
                    Response.Status = ResponseStatusEnum.Success;
                    return Response;
                }

                var task = await _context.Task
                    .Include(t => t.OwnerUser)
                    .Include(t => t.ResponsibleUser)
                    .Include(t => t.Category)
                    .Include(t => t.Space)
                    .Include(x=>x.ChildTasks)
                    .FirstOrDefaultAsync(t => t.Id == TaskId);

                if (task is null)
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "Erro. Tarefa não encontrada.";
                    return Response;
                }

                await _cachingPort.SetAsync($"{KeysCachingEnum.Task}_{TaskId}", task, TimeSpan.FromMinutes(5));

                Response.Status = ResponseStatusEnum.Success;
                Response.Content = task;
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
