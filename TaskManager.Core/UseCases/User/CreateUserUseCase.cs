using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Ports.User;
using TaskManager.Core.UseCases.User.Interfaces;

namespace TaskManager.Core.UseCases.User
{
    internal class CreateUserUseCase : ICreateUserUseCase
    {

        private readonly ICreateUserPort _createUserPort;
        public CreateUserUseCase(ICreateUserPort createUserPort)
        {
            _createUserPort = createUserPort;
        }
    }
}
