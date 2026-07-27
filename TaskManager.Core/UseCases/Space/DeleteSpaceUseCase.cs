using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Notifications;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class DeleteSpaceUseCase : IDeleteSpaceUseCase
    {
        private readonly IDeleteSpacePort _deleteSpacePort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly ISignalRNotifier _notifier;
        public DeleteSpaceUseCase(IDeleteSpacePort deleteSpacePort, ICurrentUserPort currentUserPort, ISignalRNotifier notifier)
        {
            _deleteSpacePort = deleteSpacePort;
            _currentUserPort = currentUserPort;
            _notifier = notifier;
        }
        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId)
        {
            var Response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Sessão expirada. Realize o login novamente.";
                return Response;
            }

            Response = await _deleteSpacePort.ExecuteAsync(spaceId, _currentUserPort.UserId);

            if(Response.Status == ResponseStatusEnum.Success)
                await _notifier.NotifySpaceUpdated(spaceId);

            return Response;
        }
    }
}
