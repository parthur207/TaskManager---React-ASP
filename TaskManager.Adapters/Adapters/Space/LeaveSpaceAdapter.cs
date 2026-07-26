using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Caching;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class LeaveSpaceAdapter : ILeaveSpacePort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;
        public LeaveSpaceAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, Guid userId)
        {
            var response = new SimpleResponseModel();
            try
            {
                if (!await _context.Space.AnyAsync(x => x.Id == spaceId))
                {
                    response.Status = ResponseStatusEnum.NotFound;
                    response.Message = "Espaço não encontrado.";
                    return response;
                }

                if (await _context.Space.AnyAsync(x =>x.OwnerId == userId))
                {
                    response.Message = "O proprietário do espaço não pode sair. Considere excluir o espaço ou transferir a propriedade.";
                    response.Status = ResponseStatusEnum.Error;
                    return response;
                }

                var membership = await _context.SpaceMember
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.SpaceId == spaceId);

                if (membership is null)
                {
                    response.Status = ResponseStatusEnum.NotFound;
                    response.Message = "Você não é membro deste espaço.";
                    return response;
                }

                _context.SpaceMember.Remove(membership);
                await _context.SaveChangesAsync();

                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Space}_{spaceId}");
                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Spaces_SideBar}_{userId}");
                await _cachingPort.RemoveAsync($"{KeysCachingEnum.Space_Members}_{spaceId}");

                response.Message = "Você saiu do espaço com sucesso.";
                response.Status = ResponseStatusEnum.Success;
                return response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, $"Erro: {ex.Message}");
                throw new Exception("Ocorreu um erro inesperado ao sair do espaço.");
            }
        }
    }
}