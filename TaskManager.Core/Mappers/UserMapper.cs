using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Models;

namespace TaskManager.Adapters.Mappers
{
    public class UserMapper
    {
        public static UserEntity ModelToEntity(CreateUserModel model)
        {
            return new UserEntity
                (
                    model.Name,
                    model.Email,
                    model.Password
                );
        }

        public static UserEntity LoginModelToEntity(LoginRequestModel model)
        {
            return new UserEntity
                (
                    model.Email,
                    model.Password
                );
        }

        public static UserDTO EntityToDTO(UserEntity entity)
        {
            return new UserDTO
                (
                    entity.Name,
                    entity.Email.Value,
                    TaskMapper.EntityToDTO(entity.Tasks),
                    entity.Role,
                    entity.Status
                );
        }
    }
}
