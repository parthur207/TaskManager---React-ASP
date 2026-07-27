using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.Space.Interfaces;

namespace TaskManager.Core.UseCases.Space
{
    public class GetSpaceDataSideBarUseCase : IGetSpaceDataSideBarUseCase
    {
        private readonly IGetSpaceDataSideBarPort _getSpaceDataSideBarPort;
        private readonly ICurrentUserPort _currentUserPort;
        public GetSpaceDataSideBarUseCase(IGetSpaceDataSideBarPort getSpaceDataSideBarPort, ICurrentUserPort currentUserPort)
        {
            _getSpaceDataSideBarPort = getSpaceDataSideBarPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<IEnumerable<SpaceItemDTO>>> ExecuteAsync()
        {
            var Response= new ResponseModel<IEnumerable<SpaceItemDTO>>();
            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Sessão expirada. Realize o login novamente.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }
            var responseRepository= await _getSpaceDataSideBarPort.ExecuteAsync(_currentUserPort.UserId);

            Response.Status= responseRepository.Status;
            Response.Message = responseRepository.Message;
            Response.Content = SpaceMapper.ListEntityToListItemDTO(responseRepository.Content);
            return Response;
        }
    }
}
