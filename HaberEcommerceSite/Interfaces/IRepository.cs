using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HaberEcommerceSite.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get();

        IEnumerable<TEntity> Get(string includeProperties);

        IQueryable<TEntity> GetQuery(string includeProperties, Expression<Func<TEntity, bool>> filter = null);

        void Insert(TEntity entity);

        TEntity Find(Guid ID);

        TEntity FindById(string includeProperties, Expression<Func<TEntity, bool>> filter = null);

        void Delete(Guid ID);

        void Delete(TEntity entity);

        void Update(TEntity entity);
    }
}
