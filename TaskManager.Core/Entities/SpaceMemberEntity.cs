using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Entities
{
    public class SpaceMemberEntity
    {
        public Guid SpaceId { get; private set; }
        public SpaceEntity Space { get; private set; }

        public Guid UserId { get; private set; }
        public UserEntity User { get; private set; }

        public DateTime JoinedAt { get; private set; }
        public bool IsAdmin { get; private set; }

        public SpaceMemberEntity(Guid spaceId, Guid userId, bool isAdmin = false)
        {
            SpaceId = spaceId;
            UserId = userId;
            JoinedAt = DateTime.UtcNow;
            IsAdmin = isAdmin;
        }
    }
}
}
