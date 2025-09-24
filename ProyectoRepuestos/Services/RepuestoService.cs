using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;

namespace ProyectoRepuestos.Services;

public class RepuestoService : BaseService<Repuesto>, IRepuestoService
{
    public RepuestoService(IBaseRepository<Repuesto> repuestoRepository) : base(repuestoRepository)
    {
    }
    new public async Task<Repuesto> CreateAsync(Repuesto entity)
    {
        var exists = await _repository.ExistsAsync(r => r.Name == entity.Name);
        if (exists)
        {
            throw new InvalidOperationException(Messages.Repuesto.AlreadyExists);
        }

        await _repository.CreateAsync(entity);
        return entity;
    }

    new public async Task<Repuesto?> UpdateAsync(int id, Repuesto entity)
    {
        var exists = await _repository.ExistsAsync(r => r.Name == entity.Name);
        var repuesto = await _repository.GetByIdAsync(id);
        if (repuesto != null && !exists)
        {
            return await _repository.UpdateAsync(id, entity);
        }
        return repuesto;
    }
}
