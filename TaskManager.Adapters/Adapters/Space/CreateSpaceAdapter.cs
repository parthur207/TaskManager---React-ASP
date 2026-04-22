using System.Diagnostics;
using TaskManager.Adapters.Persistence;
using TaskManager.Core.Entities;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Persistence.Space;
using TaskManager.Core.ResposePattern;

namespace TaskManager.Adapters.Adapters.Space
{
    public class CreateSpaceAdapter : ICreateSpacePort
    {
        private readonly DbContextTaskManager _context;

        public CreateSpaceAdapter(DbContextTaskManager context)
        {
            _context = context;
        }

        public async Task<SimpleResponseModel> ExecuteAsync(SpaceEntity entity, Guid userId)
        {
            var response = new SimpleResponseModel();
            try
            {
                if (entity is null)
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "Entidade do espaço inválida.";
                    return response;
                }

                var alreadyExists = _context.Space
                    .Any(s => s.OwnerId == userId && s.Name == entity.Name);

                if (alreadyExists)
                {
                    response.Status = ResponseStatusEnum.Error;
                    response.Message = "Já existe um espaço com este nome para o usuário.";
                    return response;
                }

                await _context.Space.AddAsync(entity);
                await _context.SaveChangesAsync();

                response.Status = ResponseStatusEnum.Success;
                response.Message = "Espaço criado com sucesso.";
                return response;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                throw new Exception($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }
    }
}