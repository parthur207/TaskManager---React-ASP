using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.User
{
    internal class DeleteUserAdapter : IDeleteUserPort
    {
        private readonly DbContextTaskManager _context;

        public DeleteUserAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(Guid userId)
        {
            var response = new SimpleResponseModel();
            try
            {
                if (userId == Guid.Empty)
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "ID de usuário inválido.";
                    return response;
                }

                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId
                    && u.Status == UserStatusEnum.Active);

                if (user is null)
                {
                    response.Status = ResponseStatusEnum.NotFound;
                    response.Message = "Usuário não encontrado ou já inativado.";
                    return response;
                }

                user.Inactive();
                _context.User.Update(user);
                await _context.SaveChangesAsync();

                response.Status = ResponseStatusEnum.Success;
                response.Message = "Conta inativada com sucesso.";
                return response;
            }
            catch (ArgumentException ex)
            {
                response.Status = ResponseStatusEnum.Error;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }
    }
}