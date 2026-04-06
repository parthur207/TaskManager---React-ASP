using System.Security.Cryptography;
using TaskManager.Core.Enums;
using TaskManager.Core.Models;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.Ports.Task;
using TaskManager.Core.ResposePattern;
using TaskManager.Core.UseCases.Task.Interfaces;

namespace TaskManager.Core.UseCases.Task
{
    public class CreateTaskUseCase : ICreateTaskUseCase
    {
        private readonly ICreateTaskPort _createTaskPort;
        private readonly IUserQueryPort _userQuery;
        private readonly ISpaceMembershipPort _membership;
        public CreateTaskUseCase(ICreateTaskPort createTaskPort, IUserQueryPort userQuery, ISpaceMembershipPort membership)
        {
            _createTaskPort = createTaskPort;
            _userQuery = userQuery;
            _membership = membership;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateTaskModel model)
        {
            var Response = new SimpleResponseModel();

            if (model is null)
            {
                Response.Message = "Erro. Dados nulos.";
                Response.Status = ResponseStatusEnum.Error;
                return Response;
            }
            if (!string.IsNullOrWhiteSpace(model.ResponsibleEmail)
                && _userQuery.UserExists(model.ResponsibleEmail).Result.Content)
            {                
                
            }

            var responseRepository = await _createTaskPort.ExecuteAsync(model);

            if (responseRepository.S)
            {

            }

            return Response;
        }
    }
}
