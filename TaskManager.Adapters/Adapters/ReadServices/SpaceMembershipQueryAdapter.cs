using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.DTOs;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.ReadServices
{
    public class SpaceMembershipQueryAdapter : ISpaceMembershipQueryPort
    {
        private readonly DbContextTaskManager _context;       
        public SpaceMembershipQueryAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<ResponseModel<IEnumerable<string>>> GetUsersEmailsInSpaceAsync(Guid spaceId, Guid userId)
        {
            var Response = new ResponseModel<IEnumerable<string>>();
            try
            {
                var isMemberResponse = await IsUserMemberAsync(userId, spaceId);
                if (!isMemberResponse.Content)
                {
                    Response.Message = "O usuário não é membro do espaço especificado.";
                    Response.Status = ResponseStatusEnum.Unauthorized;
                    return Response;
                }

                var usersEmails = await _context.SpaceMember
                    .Where(x => x.SpaceId == spaceId)
                    .Select(x => x.User.Email.Value)
                    .ToListAsync();

                if (usersEmails == null || !usersEmails.Any())
                {
                    Response.Message = "Nenhum usuário encontrado no espaço especificado.";
                    Response.Status = ResponseStatusEnum.NotFound;
                    return Response;
                }

                Response.Content = usersEmails;
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado.");
                Debug.Assert(false, ex.Message);
            }
        }

      

        public async Task<ResponseModel<bool>> IsUserMemberAsync(Guid userId, Guid spaceId)
        {
            var Response = new ResponseModel<bool>();
            try
            {
                if (!await _context.SpaceMember.AnyAsync(x => x.SpaceId == spaceId 
                && x.UserId == userId))
                {
                    Response.Status = ResponseStatusEnum.NotFound;
                    Response.Message = "O usuário não é membro do espaço especificado.";
                    Response.Content = false;
                    return Response;
                }

                Response.Status = ResponseStatusEnum.Success;
                Response.Content = true;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro inesperado.");
                Debug.Assert(false, ex.Message);
            }
        }
    }
}
