using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.ReadServices
{
    public interface IUserQueryPort
    {
        Task<ResponseModel<UserEntity?>> GetUserByEmailAsync(string email);
        Task<ResponseModel<bool>> UserExists(string email);
        Task<ResponseModel<IQueryable<Guid>>> GetUserIdByEmail(string email);

    }
}
