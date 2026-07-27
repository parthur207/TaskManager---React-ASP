using System.Diagnostics;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Task;
using TaskManager.Core.Ports.Security;
using TaskManager.Core.ResponsePattern;

namespace TaskManager.Adapters.Adapters.Task
{
    public class CreateTaskAdapter : ICreateTaskPort
    {
        private readonly DbContextTaskManager _context;
        public CreateTaskAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(TaskEntity entity)
        {
            var Response = new SimpleResponseModel();
            try
            {
                if (entity is null)
                {
                    Response.Status= ResponseStatusEnum.Error;
                    Response.Message="Ocorreu um erro. Entidade inválida/nula";
                    return Response;
                }

                await _context.Task.AddAsync(entity);
                await _context.SaveChangesAsync();

                Response.Status = ResponseStatusEnum.Success;
                Response.Message = "Tarefa criada com sucesso.";
                return Response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, "Erro:" + ex.Message);
                throw new Exception("Ocorreu um erro inesperado.");
            }
        }
    }
}
