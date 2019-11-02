using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blobzor.Core.Model.Domain;

namespace Blobzor.Core.Interface
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        void Add(TEntity entity);
        void Add(IEnumerable<TEntity> entities);
        void Add(params TEntity[] entities);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes);
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> GetAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> GetAsync(int Id, params Expression<Func<TEntity, object>>[] includes);

        Task SaveAsync();

        Task ReloadAsync();
    }
}