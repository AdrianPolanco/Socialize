
using Socialize.Core.Domain.Entities.Base;
using Socialize.Core.Domain.Repositories.Base;
using Socialize.Infrastructure.Identity.Context;

namespace Socialize.Infrastructure.Identity.Repositories.Base
{
    public class Repository<T> : PartialRepository<T>, IRepository<T> where T : Entity
    {
        public Repository(ApplicationDbContext context) : base(context)
        {
        }

        public virtual async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            using (await BeginTransactionAsync())
            {
                try
                {
                    T? entity = await GetByIdAsync(id, cancellationToken);
                    if (entity == null) return;
                    entity.Deleted = true;
                    await UpdateAsync(entity, cancellationToken);
                    await SaveChangesAsync(cancellationToken);
                    await CommitAsync(cancellationToken);
                }
                catch (Exception e)
                {
                    await RollbackAsync(cancellationToken);
                    throw e;
                }
            }
        }

        public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            using (await BeginTransactionAsync())
            { 
                try
                {
                    _context.Set<T>().Update(entity);
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
    }
}
