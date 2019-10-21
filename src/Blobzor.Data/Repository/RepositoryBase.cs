using System;
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
    }
}