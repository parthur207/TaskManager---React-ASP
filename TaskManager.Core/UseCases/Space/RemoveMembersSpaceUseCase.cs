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
    public class RemoveMembersSpaceUseCase : IRemoveMembersSpaceUseCase
    {
        private readonly IRemoveMembersSpacePort _removeMembersSpacePort;
        private readonly ICurrentUserPort _currentUserPort;
        private readonly ISignalRNotifier _notifier;
        public RemoveMembersSpaceUseCase(IRemoveMembersSpacePort removeMembersSpacePort, ICurrentUserPort currentUserPort, ISignalRNotifier notifier)
        {
            _removeMembersSpacePort = removeMembersSpacePort;
            _currentUserPort = currentUserPort;
            _notifier = notifier;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, ICollection<string> MembersEmails)
        {
            var Response = new SimpleResponseModel();

            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Status = ResponseStatusEnum.Unauthorized;
                Response.Message = "Sessão expirada. Realize o login novamente.";
                return Response;
            }


            if (MembersEmails is null || !MembersEmails.Any())
            {
                Response.Message= "Erro. A lista de emails dos membros a serem removidos não pode ser vazia.";
                Response.Status = ResponseStatusEnum.Error;
                return Response;
            }

            var responseRepository = await _removeMembersSpacePort.ExecuteAsync(spaceId, _currentUserPort.UserId, MembersEmails);

            if(responseRepository.Status == ResponseStatusEnum.Success)
                await _notifier.NotifySpaceUpdated(spaceId);

            return responseRepository;
        }
    }
}
