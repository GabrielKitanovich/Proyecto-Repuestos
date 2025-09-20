using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Helpers;
using ProyectoRepuestos.Models;

namespace ProyectoRepuestos.Services;

public class RepuestoService : BaseService, IRepuestoService
{

    private readonly ApplicationDbContext _context;
    private readonly DbSet<Repuesto> _repuestos;

    public RepuestoService(ApplicationDbContext context)
    {
        _context = context;
        _repuestos = _context.Set<Repuesto>();
    }

    public async Task<Repuesto> CreateAsync(Repuesto entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var repuesto = await _repuestos.FindAsync(id);
        if (repuesto == null)
            return false;
        _context.Repuestos.Remove(repuesto);
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<List<Repuesto>> GetAllAsync()
    {
        return await _repuestos.ToListAsync();
    }

    public async Task<Repuesto?> GetByIdAsync(int id)
    {

        return await _repuestos.FindAsync(id);
    }

    public async Task<Repuesto?> UpdateAsync(int id, Repuesto entity)
    {
        var repuesto = await _repuestos.FindAsync(id);
        if (repuesto != null)
        {
            _context.Repuestos.Update(entity);
            await _context.SaveChangesAsync();
            return await _repuestos.FindAsync(id);
        }
        return repuesto;
    }
    /*
    public ActionResult? RepuestoExists(int repuestoId, out Repuesto? repuesto)
    {
        return EntityExists(_repuestos, repuestoId, out repuesto, Messages.Repuesto.NotFound);
    }
    */
}
