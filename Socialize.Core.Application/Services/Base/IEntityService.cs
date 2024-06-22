﻿

using System.Linq.Expressions;

namespace Socialize.Core.Application.Services.Base
{
    public interface IEntityService<T> where T : class
    {
       // Task<ICollection<T>> GetAsync(CancellationToken cancellationToken, bool readOnly = false, Expression<Func<T, object>>[] includes = null);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool readOnly = false, Expression<Func<T, object>>[] includes = null);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
