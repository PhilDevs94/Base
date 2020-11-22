using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Core.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Find(object Id);
        TEntity Find(params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(object entity);
        IQueryable<TEntity> Queryable();
    }
}
