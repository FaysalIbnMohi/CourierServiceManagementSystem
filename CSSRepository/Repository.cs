using CSSEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CSSRepository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        DataContext context = DataContext.getInstance();

        public List<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public TEntity Get(string id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public int Insert(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            return context.SaveChanges();
        }

        public int Update(TEntity entity)
        {
            context.Entry<TEntity>(entity).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public int Delete(string id)
        {
            TEntity entity = Get(id);
            context.Set<TEntity>().Remove(entity);
            return context.SaveChanges();
        }
    }
}
