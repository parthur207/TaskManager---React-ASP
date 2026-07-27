using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class AssignExecutionPlanTaskAdapter : IAssignExecutionPlanTaskPort
    {

        private readonly DbContextTaskManager _context;

        public AssignExecutionPlanTaskAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(string? executionPlan)
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
                return Response;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
