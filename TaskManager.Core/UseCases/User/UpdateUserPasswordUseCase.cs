using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.User;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.UseCases.User
{
    internal class UpdateUserPasswordUseCase : IUpdateUserPassworkUseCase
    {
        private readonly IUpdateUserPasswordPort _updateUserPasswordPort;
        public UpdateUserPasswordUseCase(IUpdateUserPasswordPort updateUserPasswordPort)
        {
            _updateUserPasswordPort = updateUserPasswordPort;
        }
    }
}
