using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class GetSpaceDataSideBarAdapter : IGetSpaceDataSideBarPort
    {
        private readonly DbContextTaskManager _context;
        private readonly ICachingPort _cachingPort;
        public GetSpaceDataSideBarAdapter(DbContextTaskManager context, ICachingPort cachingPort)
        {
            _context = context;
            _cachingPort = cachingPort;
        }

        public async Task<ResponseModel<IEnumerable<SpaceEntity>>> ExecuteAsync(Guid userId)
        {
            var Response = new ResponseModel<IEnumerable<SpaceEntity>>();
            try
            {
                if (!await _context.User.AnyAsync(u => u.Id == userId))
                {
                    Response.Message = "Usuário não encontrado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                if (!await _context.SpaceMember.AnyAsync(s => s.UserId == userId))
                {
                    Response.Message = "Usuário não é membro de nenhum espaço.";
                    Response.Status = ResponseStatusEnum.NotFound;
                    return Response;
                }

                var responseCache = await _cachingPort.GetAsync<IEnumerable<SpaceEntity>>($"{KeysCachingEnum.Spaces_SideBar}_{userId}");

                if (responseCache!=null)
                {
                    Response.Content = responseCache;
                    Response.Status = ResponseStatusEnum.Success;
                    return Response;
                }

                var spaces= await _context.Space
                    .Where(x=>x.Members
                    .Any(x=>x.UserId==userId))
                    .ToListAsync();

                await _cachingPort.SetAsync($"{KeysCachingEnum.Spaces_SideBar}_{userId}", spaces, TimeSpan.FromMinutes(5));

                Response.Content= spaces;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch(Exception ex)
            {
                Debug.Assert(false, "erro: "+ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
