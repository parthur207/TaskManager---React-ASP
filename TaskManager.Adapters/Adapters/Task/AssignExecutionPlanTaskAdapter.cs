using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.ReadServices;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class AssignExecutionPlanTaskAdapter : IAssignExecutionPlanTaskPort
    {

        private readonly DbContextTaskManager _context;
        private readonly ISpaceMembershipQueryPort _spaceMembershipQueryPort;
        public AssignExecutionPlanTaskAdapter(DbContextTaskManager context, ISpaceMembershipQueryPort spaceMembershipQueryPort)
        {
            _context = context;
            _spaceMembershipQueryPort = spaceMembershipQueryPort;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(string? executionPlan, Guid taskId, Guid userId)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (string.IsNullOrWhiteSpace(executionPlan))
                {
                    Response.Message= "Erro. Falha na atribuição. O plano de execução não pode ser nulo ou vazio.";
                    Response.Status = ResponseStatusEnum.Error;
                    return Response;
                }

                var IsMember = await _spaceMembershipQueryPort.IsUserMemberAsync(taskId, userId);

                if (!await _context.Space.AnyAsync(x=>x.Members
                        .Any(x=>x.UserId == userId) 
                        && x.Tasks.Any(t=>t.Id==taskId)))
                {
                    Response.Message= "Erro. Falha na atribuição. O usuário não é membro do espaço associado à tarefa.";
                    Response.Status= ResponseStatusEnum.Unauthorized;
                    return Response;
                }

                var task = await _context.Task.FirstOrDefaultAsync(x => x.Id == taskId);

                task.AssignExecutionPlan(executionPlan);
                _context.Task.Update(task);
                await _context.SaveChangesAsync();

                Response.Message = "Plano de execução atribuído com sucesso.";
                Response.Status = ResponseStatusEnum.Success;
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
