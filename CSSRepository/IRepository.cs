using CSSEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSSRepository
{
    public interface IRepository<TEntity> where TEntity : Entity 
    {
        List<TEntity> GetAll();
        TEntity Get(string id);
        int Insert(TEntity entity);
        int Update(TEntity entity);
        int Delete(string id);
    }
}
