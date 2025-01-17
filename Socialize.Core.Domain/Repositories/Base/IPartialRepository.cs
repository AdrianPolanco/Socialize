﻿using Socialize.Core.Domain.Entities.Base;
using System.Linq.Expressions;

namespace Socialize.Core.Domain.Repositories.Base
{
    public interface IPartialRepository<T> where T : Entity
    {
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool readOnly = false, bool ignoreQueryFilters = false, params Expression<Func<T, object>>[] includes);
        Task<ICollection<T>> GetByPagesAsync(Guid? lastId, CancellationToken cancellationToken, bool readOnly = true, bool ignoreQueryFilters = false, Expression<Func<T, bool>> filter = null,
            params Expression<Func<T, object>>[] includes);
        Task<ICollection<T>> GetByFilter(Expression<Func<T, bool>> filter, CancellationToken cancellationToken, bool readOnly = true, bool ignoreQueryFilters = false, Expression<Func<T, object>>[] includes = null);
    }
}
