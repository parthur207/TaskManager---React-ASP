using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class GetUsersBySpaceIdUseCase : IGetUsersBySpaceIdUseCase
    {
        private readonly ICurrentUserPort _currentUserPort;
        private readonly IGetUsersBySpaceIdPort _getUsersBySpaceIdPort;
        public GetUsersBySpaceIdUseCase(ICurrentUserPort currentUserPort, IGetUsersBySpaceIdPort getUsersBySpaceIdPort)
        {
            _currentUserPort = currentUserPort;
            _getUsersBySpaceIdPort = getUsersBySpaceIdPort;
        }

        public async Task<ResponseModel<IEnumerable<string>>> ExecuteAsync(Guid spaceId)
        {
            var Response = new ResponseModel<IEnumerable<string>>();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Sessão expirada. Realize o login novamente.";
                return Response;
            }

            var ResponsePort = await _getUsersBySpaceIdPort.ExecuteAsync(spaceId);

            return ResponsePort;
        }
    }
}
