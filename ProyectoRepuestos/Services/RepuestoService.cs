using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Repositories.Interfaces;

namespace ProyectoRepuestos.Services;

public class RepuestoService : BaseService, IRepuestoService
{

    private readonly IRepuestoRepository _repuestoRepository;

    public RepuestoService(IRepuestoRepository repuestoRepository)
    {
        _repuestoRepository = repuestoRepository;
    }
    

    public async Task<Repuesto> CreateAsync(Repuesto entity)
    {
        var exists = await _repuestoRepository.ExistsAsync(r => r.Name == entity.Name);
        if (exists)
        {
            throw new InvalidOperationException(Messages.Repuesto.AlreadyExists);
        }

        await _repuestoRepository.CreateAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var repuesto = await _repuestoRepository.GetByIdAsync(id);
        if (repuesto == null)
            return false;
        await _repuestoRepository.DeleteAsync(id);
        return true;
    }


    public async Task<List<Repuesto>> GetAllAsync()
    {
        return await _repuestoRepository.GetAllAsync();
    }

    public async Task<Repuesto?> GetByIdAsync(int id)
    {

        return await _repuestoRepository.GetByIdAsync(id);
    }

    public async Task<Repuesto?> UpdateAsync(int id, Repuesto entity)
    {
        var repuesto = await _repuestoRepository.GetByIdAsync(id);
        if (repuesto != null)
        {
            return await _repuestoRepository.UpdateAsync(id, entity);
        }
        return repuesto;
    }
}
