﻿
using Socialize.Core.Domain.Entities.Base;
using Socialize.Core.Domain.Repositories.Base;
using System.Linq.Expressions;

namespace Socialize.Core.Application.Services.Base
{
    public class EntityService<T> : IEntityService<T> where T : Entity
    {
        private readonly IRepository<T> _repository;

        public EntityService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            return await _repository.CreateAsync(entity, cancellationToken);
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            return await _repository.UpdateAsync(entity, cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool readOnly, bool ignoreQueryFilters = false, Expression<Func<T, object>>[] includes = null)
        {
            return await _repository.GetByIdAsync(id, cancellationToken, readOnly, ignoreQueryFilters, includes);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(id, cancellationToken);
        }

        public async Task<ICollection<T>> GetByFilter(Expression<Func<T, bool>> filter, CancellationToken cancellationToken, bool readOnly = true, bool ignoreQueryFilters = false, Expression<Func<T, object>>[] includes = null)
        {
            return await _repository.GetByFilter(filter, cancellationToken, readOnly, ignoreQueryFilters, includes);
        }
    }
}
