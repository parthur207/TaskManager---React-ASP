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
    public class GetSpaceDetailsByIdUseCase : IGetSpaceDetailsByIdUseCase
    {

        private readonly IGetSpaceDetailsByIdPort _getSpacesIdByUserIdPort;
        private readonly ICurrentUserPort _currentUserPort;
        public GetSpaceDetailsByIdUseCase(IGetSpaceDetailsByIdPort getSpacesIdByUserIdPort, ICurrentUserPort currentUserPort)
        {
            _getSpacesIdByUserIdPort = getSpacesIdByUserIdPort;
            _currentUserPort = currentUserPort;
        }

        public async Task<ResponseModel<SpaceDTO>> ExecuteAsync(Guid spaceId)
        {
            var Response = new ResponseModel<SpaceDTO>();  
            
            if (!_currentUserPort.IsAuthenticated)
            {
                Response.Message = "Sessão expirada. Realize o login novamente.";
                Response.Status = ResponseStatusEnum.Unauthorized;
                return Response;
            }

            var responseRepository = await _getSpacesIdByUserIdPort.ExecuteAsync(_currentUserPort.UserId, spaceId);

            if (responseRepository.Status != ResponseStatusEnum.Success)
            {
                Response.Status=responseRepository.Status;
                Response.Message = responseRepository.Message;
                return Response;
            }

            Response.Content = SpaceMapper.EntityToDTO(responseRepository.Content);
            Response.Status = responseRepository.Status;
            return Response;
        }
    }
}
