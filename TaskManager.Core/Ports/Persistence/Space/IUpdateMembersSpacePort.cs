using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Core.Ports.Persistence.Space
{
    public interface IUpdateMembersSpacePort
    {

        Task<SimpleResponseModel> ExecuteAsync(Guid spaceId, IList<Guid> UsersId);
    }
}
