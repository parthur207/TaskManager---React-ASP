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

namespace TaskManager.Adapters.Adapters.User
{
    public class GetUsersBySpaceIdAdapter : IGetUsersBySpaceIdPort
    {
        private readonly DbContextTaskManager _dbContextTaskManager;
        private readonly ICachingPort _cachingPort;

        public GetUsersBySpaceIdAdapter(DbContextTaskManager dbContextTaskManager, ICachingPort cachingPort)
        {
            _dbContextTaskManager = dbContextTaskManager;
            _cachingPort = cachingPort;
        }

        public async Task<ResponseModel<IEnumerable<string>>> ExecuteAsync(Guid spaceId)
        {
            var Response= new ResponseModel<IEnumerable<string>>();
            try
            {
                if (!await _dbContextTaskManager.Space.AnyAsync(s => s.Id == spaceId))
                {
                    Response.Message = "Erro. Espaço não encontrado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                var responseCache = await _cachingPort.GetAsync<IEnumerable<string>>($"{KeysCachingEnum.Space_Members}_{spaceId}");

                if (responseCache != null)
                {
                    Response.Status = ResponseStatusEnum.Success;
                    Response.Content = responseCache;
                    return Response;
                }

                var users =await _dbContextTaskManager.SpaceMember.Where(u => u.SpaceId == spaceId).Select(u => u.User.Email.Value).ToListAsync();

                if (users is null || !users.Any())
                {
                    Response.Message = "Nenhum usuário encontrado para o espaço especificado.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                await _cachingPort.SetAsync($"{KeysCachingEnum.Space_Members}_{spaceId}", users, TimeSpan.FromMinutes(5));

                Response.Content = users;
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
