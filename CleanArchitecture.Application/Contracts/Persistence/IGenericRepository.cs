using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Contracts.Persistence
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }

        ValueTask<TEntity> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true);
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true);
        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true);
        Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default, bool saveNow = true);
        Task<bool> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default, bool saveNow = true);

        TEntity GetById(params object[] ids);
        TEntity Add(TEntity entity, bool saveNow = true);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, bool saveNow = true);
        TEntity Update(TEntity entity, bool saveNow = true);
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true);
        bool Delete(TEntity entity, bool saveNow = true);
        bool DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true);

        void Detach(TEntity entity);
        void Attach(TEntity entity);

        Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
            where TProperty : class;
        void LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
            where TProperty : class;
        Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken)
            where TProperty : class;
        void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty)
            where TProperty : class;
    }
}
