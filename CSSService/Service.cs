using CSSEntity;
using CSSRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CSSService
{
    public class Service<TEntity> : IService<TEntity> where TEntity : Entity
    {
        IRepository<TEntity> repo = new Repository<TEntity>();

        public List<TEntity> GetAll()
        {
            return repo.GetAll();
        }

        public TEntity Get(string id)
        {
            return repo.Get(id);
        }

        public int Insert(TEntity entity)
        {
            return repo.Insert(entity);
        }

        public int Update(TEntity entity)
        {
            return repo.Update(entity);
        }

        public int Delete(string id)
        {
            return repo.Delete(id);
        }
    }
}
