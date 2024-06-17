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
        protected readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        public PartialRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            using var context = _dbContextFactory.CreateDbContext();
            try
            {
                await BeginTransactionAsync(context, cancellationToken);
                await context.Set<T>().AddAsync(entity, cancellationToken);
                await SaveChangesAsync(context, cancellationToken);
                await CommitAsync(context, cancellationToken);
                return entity;
            }catch(Exception e)
            {
                await RollbackAsync(context, cancellationToken);
                throw e;
            }
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool readOnly = false, params Expression<Func<T, object>>[] includes)
        {
            try
            {
                using var context = _dbContextFactory.CreateDbContext();
                DbSet<T> _dbSet = context.Set<T>();
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

        public async Task<ICollection<T>> GetByPagesAsync(Guid lastId, CancellationToken cancellationToken, bool readOnly = true, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes)
        { 
            int pageSize = 10;
            using var context = _dbContextFactory.CreateDbContext();
            DbSet<T> _dbSet = context.Set<T>();
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

        protected async Task<IDbContextTransaction> BeginTransactionAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            return await context.Database.BeginTransactionAsync();
        }

        protected async Task CommitAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            await context.Database.CommitTransactionAsync(cancellationToken);
        }

        protected async Task<int> SaveChangesAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }

        protected async Task RollbackAsync(ApplicationDbContext context, CancellationToken cancellationToken)
        {
            if (context.Database.CurrentTransaction != null)
            {
                await context.Database.RollbackTransactionAsync(cancellationToken);
            }
        }
    }
}
