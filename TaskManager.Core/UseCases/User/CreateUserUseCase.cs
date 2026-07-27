using TaskManager.Core.Enums;
using TaskManager.Core.Models.User;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.ResponsePattern;
using TaskManager.Core.UseCases.User.Interfaces;
using TaskManager.Core.ValueObjects;

namespace TaskManager.Core.UseCases.User
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly ICreateUserPort _createUserPort;
        
        public CreateUserUseCase(ICreateUserPort createUserPort)
        {
            _createUserPort = createUserPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateUserModel model)
        {
            var Response = new SimpleResponseModel();
            
            if (model is null)
            {
                Response.Message = "Erro. Modelo de criação nula.";
                Response.Status = ResponseStatusEnum.Error;
                return Response;
            }

            if (model.Password != model.PasswordConfirmation)
            {
                Response.Message = "Erro. As senhas informadas não se correspondem.";
                Response.Status = ResponseStatusEnum.Error;
                return Response;
            }

            var passwordVO= new PasswordVO(model.Password, false, false);
            model.Password = passwordVO.Value;

            var resultRepository= await _createUserPort.ExecuteAsync(model);

            if (resultRepository.Status!=ResponseStatusEnum.Success)
            {
                Response.Status=resultRepository.Status;
                Response.Message = resultRepository.Message;
                return Response;
            }

            return Response=resultRepository;
        }
    }
}
