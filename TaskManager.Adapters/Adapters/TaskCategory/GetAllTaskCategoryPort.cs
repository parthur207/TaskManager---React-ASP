using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
using TaskManager.Core.Ports.Persistence.TaskCategory;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.TaskCategory.Interfaces;

namespace TaskManager.Adapters.Adapters.TaskCategory
{
    public class GetAllTaskCategoryPort : IGetAllTaskCategoryPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ISpaceMembershipQueryPort _spaceMembershipQueryPort;
        private readonly ICachingPort _cachingPort;

        public GetAllTaskCategoryPort(DbContextTaskManager context, ISpaceMembershipQueryPort spaceMembershipQueryPort, ICachingPort cachingPort)
        {
            _context = context;
            _spaceMembershipQueryPort = spaceMembershipQueryPort;
            _cachingPort = cachingPort;
        }

        public async Task<ResponseModel<IEnumerable<TaskCategoryEntity>>> ExecuteAsync(Guid spaceId, Guid userId)
        {
            var Response= new ResponseModel<IEnumerable<TaskCategoryEntity>>();
            try
            {
                var IsMemberResponse = await _spaceMembershipQueryPort.IsUserMemberAsync(userId, spaceId);

                if (!IsMemberResponse.Content)
                {
                    Response.Message = "Você não possui permissão para este espaço.";
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    return Response;
                }

                if (!await _context.TaskCategory.AnyAsync(x => x.SpaceId == spaceId))
                {
                    Response.Message = "Nenhuma categoria de tarefa encontrada para o espaço especificado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                var responseCache = await _cachingPort
                    .GetAsync<IEnumerable<TaskCategoryEntity>>($"taskCategories_{spaceId}");

                if (responseCache != null)
                {
                    Response.Status = ResponseStatusEnum.Success;
                    Response.Content = responseCache;
                    return Response;
                }

                var taskCategories = await _context.TaskCategory
                    .Where(x => x.SpaceId == spaceId)
                    .ToListAsync();

                if (taskCategories is null || !taskCategories.Any())
                {
                    Response.Message = "Nenhuma categoria de tarefa encontrada para o espaço especificado.";
                    Response.Status = ResponseStatusEnum.NotFound;
                    return Response;
                }

                await _cachingPort.SetAsync($"taskCategories_{spaceId}", taskCategories, TimeSpan.FromMinutes(5));

                Response.Status= ResponseStatusEnum.Success;
                Response.Content = taskCategories;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ocorreu um erro inesperado: "+ex);
                throw new Exception("Ocorreu um erro inesperado ao tentar coletar as categorias de tarefas deste espaço.");
            }
        }
    }
}
