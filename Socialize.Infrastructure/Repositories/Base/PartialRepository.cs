using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Socialize.Core.Domain.Entities.Base;
using Socialize.Core.Domain.Repositories.Base;
using Socialize.Infrastructure.Identity.Context;
using System.Linq.Expressions;

namespace Socialize.Infrastructure.Identity.Repositories.Base
{
    public class PartialRepository<T> : IPartialRepository<T> where T : Entity
    {
        protected readonly ApplicationDbContext _context;
        public PartialRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            using (await BeginTransactionAsync()) { 
                try
                {
                    await _context.Set<T>().AddAsync(entity, cancellationToken);
                    await SaveChangesAsync(cancellationToken);
                    await CommitAsync(cancellationToken);
                    return entity;
                }
                catch (Exception e)
                {
                    await RollbackAsync(cancellationToken);
                    throw e;
                }
            }
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool readOnly = false, params Expression<Func<T, object>>[] includes)
        {
                try
                {
                    DbSet<T> _dbSet = _context.Set<T>();
                    IQueryable<T> query = _dbSet.AsQueryable();

                    // Aplicar includes opcionales
                    if (includes != null)
                    {
                        query = includes.Aggregate(query, (current, include) => current.Include(include));
                    }

                    if(readOnly) query = query.AsNoTracking();

                    T? entity = await query.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
                    return entity;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

        }

        public virtual async Task<ICollection<T>> GetByPagesAsync(Guid? lastId, CancellationToken cancellationToken, bool readOnly = true, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        { 
                int pageSize = 10;

                DbSet<T> _dbSet = _context.Set<T>();
                IQueryable<T> query = _dbSet.AsQueryable();

                query = query.OrderBy(e => e.Id);

                // Aplicar filtro si se proporciona
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                query = query.Where(e => e.Id > lastId);

                // Aplicar includes opcionales
                if (includes != null)
                {
                    query = includes.Aggregate(query, (current, include) => current.Include(include));
                }

                if(readOnly) query = query.AsNoTracking();

                return await query.Take(pageSize).ToListAsync();
        }

        protected async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        protected async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }

        protected async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        protected async Task RollbackAsync(CancellationToken cancellationToken)
        {
            if (_context.Database.CurrentTransaction != null)
            {
                await _context.Database.RollbackTransactionAsync(cancellationToken);
            }
        }
    }
}
