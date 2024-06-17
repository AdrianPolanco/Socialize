using Microsoft.EntityFrameworkCore;
using Socialize.Core.Domain.Entities.Base;
using Socialize.Core.Domain.Repositories.Base;
using Socialize.Infrastructure.Identity.Context;

namespace Socialize.Infrastructure.Identity.Repositories.Base
{
    public class Repository<T> : PartialRepository<T>, IRepository<T> where T : Entity
    {
        public Repository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            using var context = _dbContextFactory.CreateDbContext();
            try
            {
                await BeginTransactionAsync(context, cancellationToken);
                T? entity = await GetByIdAsync(id, cancellationToken);
                if (entity == null) return;
                entity.Deleted = true;
                await UpdateAsync(entity, cancellationToken);
                await SaveChangesAsync(context, cancellationToken);
                await CommitAsync(context, cancellationToken);
            }
            catch (Exception e)
            {
                await RollbackAsync(context, cancellationToken);
                throw e;
            }
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            using var context = _dbContextFactory.CreateDbContext();
            try
            {
                await BeginTransactionAsync(context, cancellationToken);
                await UpdateAsync(entity, cancellationToken);
                await SaveChangesAsync(context, cancellationToken);
                await CommitAsync(context, cancellationToken);
                return entity;
            }
            catch (Exception e)
            {
                await RollbackAsync(context, cancellationToken);
                throw e;
            }
        }
    }
}
