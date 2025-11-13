using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProyectoRepuestos.Services;

namespace ProyectoRepuestos.Bases;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T?> UpdateAsync(int id, T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null && entity is BaseModel baseEntity)
        {
            baseEntity.IsActive = false;
            baseEntity.DeletedAt = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<T?> RestoreAsync(int id)
    {
        var entity = await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        if (entity != null && entity is BaseModel baseEntity)
        {
            baseEntity.IsActive = true;
            baseEntity.DeletedAt = null;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        return null;
    }
}