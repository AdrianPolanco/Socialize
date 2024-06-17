using Socialize.Core.Domain.Entities.Base;

namespace Socialize.Core.Domain.Repositories.Base
{
    public interface IRepository<T> : IPartialRepository<T> where T : Entity
    {
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
