using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class DeleteSpaceAdapter : IDeleteSpacePort
    {

        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;
        public DeleteSpaceAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, Guid userId)
        {
            var Response = new SimpleResponseModel();
            try
            {
                var spaceExists = await _context.Space
                .AnyAsync(x => x.Id == spaceId);

                if (!spaceExists)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Espaço não encontrado.";
                    return Response;
                }

                if (!await _context.Space.AnyAsync(x => x.Id == spaceId && x.OwnerId == userId))
                {
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    Response.Message = "Erro. Autorização necessária.";
                    return Response;
                }

                await _context.Space.Where(x => x.Id == spaceId).ExecuteDeleteAsync();
                await _context.SaveChangesAsync();

                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Space}_{spaceId}");
                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Spaces_SideBar}_{userId}");
                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Space_Tasks}_{spaceId}");

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Espaço excluído com sucesso.";
                return Response;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                throw new Exception("Ocorreu um erro inesperado ao tentar excluir o espaço.");
            }
        }
    }
}
