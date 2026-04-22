using TaskManager.Core.Models.User;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.Persistence.User
{
    public interface IUpdateUserPasswordPort
    {
        Task<SimpleResponseModel> ExecuteAsync(Guid userId, UpdateUserPasswordModel model);
    }
}