using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blobzor.Core.Interface;
using Blobzor.Core.Model.Domain;
using Blobzor.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Blobzor.Data.Repository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Add(params TEntity[] entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public IQueryable<TEntity> Query(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = this._dbSet;

            foreach(var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            return this.Query(includes).Where(expression);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            return await this.Query(includes).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression, params Expression<Func<TEntity, object>>[] includes)
        {
            return await this.Query(includes).Where(expression).ToListAsync();
        }

        public async Task<TEntity> GetAsync(int Id, params Expression<Func<TEntity, object>>[] includes)
        {
            return await this.Query(includes).Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await this._appDbContext.SaveChangesAsync();
        }

        public async Task ReloadAsync()
        {
            var entries = _appDbContext.ChangeTracker.Entries().ToList();

            foreach(var entry in entries)
            {
                await entry.ReloadAsync();
            }
        }
    }
}