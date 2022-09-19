using HaberEcommerceSite.Data.Entities;
using HaberEcommerceSite.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HaberEcommerceSite.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private HaberContext databaseContext;

        private DbSet<TEntity> databaseSet;

        public Repository(HaberContext context)
        {
            this.databaseContext = context;

            this.databaseSet = context.Set<TEntity>();
        }
        
        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = this.databaseSet;
            return query.ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            this.databaseSet.Add(entity);
        }

        public virtual void Delete(Guid ID)
        {
            TEntity entity = databaseSet.Find(ID);

            this.Delete(entity);
        }

        public virtual TEntity Find(Guid ID)
        {
            TEntity entity = databaseSet.Find(ID);
            
            return entity;
        }

        public virtual IEnumerable<TEntity> Get(string includeProperties)
        {
             IQueryable<TEntity> query = databaseSet;

             foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
             {
                 query = query.Include(includeProperty);
             }
           
           return query.ToList();
           
        }

        public virtual void Delete(TEntity entity)
        {
            if (this.databaseContext.Entry(entity).State is EntityState.Detached)
            {
                this.databaseSet.Attach(entity);
            }

            this.databaseSet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            this.databaseSet.Attach(entity);

            this.databaseContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual IQueryable<TEntity> GetQuery(string includeProperties, Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = databaseSet;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            return query;

        }

        public virtual TEntity FindById(string includeProperties, Expression<Func<TEntity, bool>> filter = null) {

            IQueryable<TEntity> query = databaseSet;

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query.First();

        }



    }
}
