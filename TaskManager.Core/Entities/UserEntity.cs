using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Enums;
using TaskManager.Core.ValueObjects;

namespace TaskManager.Core.Entities
{
    public class UserEntity
    {
        public UserEntity(string name, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = new EmailVO(email);
            PasswordHash = new PasswordVO(passwordHash);
            Role=RoleUserEnum.UserCommom;
            Status=UserStatusEnum.Active;
            Tasks = [];
            Spaces = [];
            CreatedAt = DateTime.Now;
        } //Create

        public UserEntity(string email, string passwordHash)
        {
            Email = new EmailVO(email);
            PasswordHash = new PasswordVO(passwordHash);
        }//Login

        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public EmailVO Email { get; private set; }
        public PasswordVO PasswordHash { get; private set; }
        public RoleUserEnum Role { get; private set; }
        public UserStatusEnum Status { get; private set; }
        public IEnumerable<TaskEntity>? Tasks { get; private set; }
        public IList<SpaceMemberEntity> Spaces { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedDate { get; private set; }
       
       public void SetPassword(string newPasswordHash, string oldPasswordHash)
       {
            if (string.IsNullOrWhiteSpace(newPasswordHash) 
                || string.IsNullOrWhiteSpace(oldPasswordHash))
                throw new ArgumentException("A nova ou a antiga senha estão nulas.");

            if (oldPasswordHash.Equals(newPasswordHash))
                throw new ArgumentException("A nova senha deve ser diferente da antiga.");

            UpdatedDate = DateTime.Now;
            PasswordHash = new PasswordVO(newPasswordHash);
       }

        public void Inactive()
        {
            if (Status.Equals(UserStatusEnum.Inactive))
                throw new ArgumentException("Cadastro já se encontra inativado");

            Status = UserStatusEnum.Inactive;
            UpdatedDate = DateTime.Now;
        }
    }
}