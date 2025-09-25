using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;

namespace ProyectoRepuestos.Services;

public class RepuestoService : BaseService<Repuesto>, IRepuestoService
{
    public RepuestoService(IBaseRepository<Repuesto> repuestoRepository) : base(repuestoRepository)
    {
    }
    public override async Task<Repuesto> CreateAsync(Repuesto entity)
    {
        var exists = await _repository.ExistsAsync(r => r.Name == entity.Name);
        if (exists)
        {
            throw new InvalidOperationException(Messages.Repuesto.AlreadyExists);
        }

        await _repository.CreateAsync(entity);
        return entity;
    }

    public override async Task<Repuesto?> UpdateAsync(int id, Repuesto entity)
    {
        var exists = await _repository.ExistsAsync(r => r.Name == entity.Name && r.Id != id);
        if (exists)
            throw new InvalidOperationException(Messages.Repuesto.AlreadyExists);

        return await _repository.UpdateAsync(id, entity);
    }
}
