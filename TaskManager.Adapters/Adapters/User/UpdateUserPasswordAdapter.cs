using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Enums;
using TaskManager.Core.Models.User;
using TaskManager.Core.Ports.Persistence.User;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Persistence.User
{
	public class UpdateUserPasswordAdapter : IUpdateUserPasswordPort
	{
		private readonly DbContextTaskManager _context;
		private readonly IPasswordHasher _passwordHasher;

		public UpdateUserPasswordAdapter(DbContextTaskManager context, IPasswordHasher passwordHasher)
		{
			_context = context;
			_passwordHasher = passwordHasher;
		}

		public async Task<SimpleResponseModel> ExecuteAsync(Guid userId, UpdateUserPasswordModel model)
		{
			var response = new SimpleResponseModel();
			try
			{
				var user = await _context.User.FirstOrDefaultAsync(u =>
					u.Id == userId && u.Status == UserStatusEnum.Active);

				if (user is null)
				{
					response.Status = ResponseStatusEnum.NotFound;
					response.Message = "Usuário não encontrado.";
					return response;
				}

				if (!_passwordHasher.Verify(model.OldPassword, user.PasswordHash.Value))
				{
					response.Status = ResponseStatusEnum.Unauthorized;
					response.Message = "A senha atual está incorreta.";
					return response;
				}

				var newHash = _passwordHasher.Hash(model.NewPassword);

				try
				{
					user.SetPassword(newHash, user.PasswordHash.Value);
				}
				catch (ArgumentException ex)
				{
					response.Status = ResponseStatusEnum.Error;
					response.Message = ex.Message;
					return response;
				}

				_context.User.Update(user);
				await _context.SaveChangesAsync();

				response.Status = ResponseStatusEnum.Success;
				response.Message = "Senha atualizada com sucesso.";
				return response;
			}
			catch (Exception ex)
			{
				throw new Exception($"Ocorreu um erro inesperado: {ex.Message}");
			}
		}
	}
}