using Microsoft.EntityFrameworkCore;
using ProyectoRepuestos.Bases;
using ProyectoRepuestos.Models;
using ProyectoRepuestos.Repositories.Interfaces;
using ProyectoRepuestos.Services;

namespace ProyectoRepuestos.Repositories;

public class RepuestoRepository(ApplicationDbContext context) : BaseRepository<Repuesto>(context), IRepuestoRepository
{
    
}