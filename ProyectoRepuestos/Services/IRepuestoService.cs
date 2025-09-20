using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Models;

namespace ProyectoRepuestos.Services
{
    public interface IRepuestoService : IBaseService
    {
    Task<List<Repuesto>> GetAllAsync();
    Task<Repuesto?> GetByIdAsync(int id);
    Task<Repuesto> CreateAsync(Repuesto entity);
    Task<Repuesto?> UpdateAsync(int id, Repuesto entity);
    Task<bool> DeleteAsync(int id);
    }
}