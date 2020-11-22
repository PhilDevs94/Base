using Base.Core.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        int ExecuteSqlCommand(string sql, params object[] parameters);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}
