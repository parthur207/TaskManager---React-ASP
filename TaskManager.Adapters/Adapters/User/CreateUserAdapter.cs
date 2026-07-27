using Microsoft.EntityFrameworkCore;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Mappers;
using TaskManager.Core.Models.User;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.User
{
    public class CreateUserAdapter : ICreateUserPort
    {
        private readonly DbContextTaskManager _context;
        private readonly IPasswordHasher _passwordHasher;
        public CreateUserAdapter(DbContextTaskManager context, IPasswordHasher passwordHasher = null)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(CreateUserModel model)
        {
            var Response = new SimpleResponseModel();
            try
            {
                model.Password = _passwordHasher.Hash(model.Password);
                var entityMapper = UserMapper.ModelToEntity(model);

                if (entityMapper is null)
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. Falha no mapeamento da entidade.";
                    return Response;
                }

                if (await _context.User.AnyAsync(
                    x => x.Email.Value == entityMapper.Email.Value))
                {
                    Response.Status = ResponseStatusEnum.Error;
                    Response.Message = "Erro. Já existe um usuário com este email.";
                    return Response;
                }

                await _context.User.AddAsync(entityMapper);
                await _context.SaveChangesAsync();

                Response.Status=ResponseStatusEnum.Success;
                Response.Message = "Cadastro realizado com sucesso.";
            }
            catch (Exception ex)
            {
                Response.Status = ResponseStatusEnum.CriticalError;
                Response.Message = $"Ocorreu um erro inesperado.{ex.Message}";
            }
            return Response;
        }
    }
}
