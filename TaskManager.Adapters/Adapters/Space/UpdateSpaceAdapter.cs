using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class UpdateSpaceAdapter : IUpdateSpacePort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;
        public UpdateSpaceAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, Guid userId, string newName)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (!await _context.Space.AnyAsync(s => s.Id == spaceId))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. O espaço não foi encontrado.";
                    return Response;
                }

                if (!await _context.Space.AnyAsync(x=>x.OwnerId==userId))
                {
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    Response.Message = "Erro. Ação não autorizada.";
                    return Response;
                }

                await _context.Space.Where(s => s.Id == spaceId)
                    .ExecuteUpdateAsync(s => s.SetProperty(s => s.Name, newName));

                await _context.SaveChangesAsync();

                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Space}_{spaceId}");

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Espaço atualizado com sucesso.";
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro: " + ex.Message);
                throw new Exception("Ocorreu um erro ao atualizar o espaço.");
            }
        }
    }
}
