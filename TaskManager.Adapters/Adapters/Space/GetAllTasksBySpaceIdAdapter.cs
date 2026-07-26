using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.VisualBasic;
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
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class GetAllTasksBySpaceIdAdapter : IGetAllTasksBySpaceIdPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;

        public GetAllTasksBySpaceIdAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<ResponseModel<IEnumerable<TaskEntity>?>> ExecuteAsync(Guid spaceId)
        {
            var Response = new ResponseModel<IEnumerable<TaskEntity>>();
            try
            {
                if (!await _context.Space.AnyAsync(x=>x.Id==spaceId))
                {
                    Response.Message = "Espaço não encontrado.";
                    Response.Status = ResponseStatusEnum.NotFound;
                    return Response;
                }

                var responseCache = await _cachingPort
                    .GetAsync<IEnumerable<TaskEntity>>($"{KeysCachingEnum.Space_Tasks}_{spaceId}");

                if (responseCache != null)
                {
                    Response.Content = responseCache;
                    Response.Status = ResponseStatusEnum.Success;
                    return Response;
                }

                var Tasks= await _context.Space
                    .Where(x=>x.Id==spaceId)
                    .SelectMany(x=>x.Tasks)
                    .ToListAsync();

                await _cachingPort.SetAsync($"{KeysCachingEnum.Space_Tasks}_{spaceId}", Tasks, TimeSpan.FromMinutes(5));

                Response.Content= Tasks;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"\nErro: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
