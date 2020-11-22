using Base.Core.DataContext;
using Base.Core.Repository;
using Base.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Base.Data
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        private readonly DataContext _context;
        protected DbTransaction Transaction;
        protected Dictionary<string, dynamic> Repositories;

        public UnitOfWork(IDataContext context)
        {
            _context = context as DataContext;
            Repositories = new Dictionary<string, dynamic>();
        }

        public virtual IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return RepositoryAsync<TEntity>();
        }

        public virtual int SaveChanges() => _context.SaveChanges();

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public virtual IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class
        {
            if (Repositories == null)
            {
                Repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (Repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)Repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            Repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, this));

            return Repositories[type];
        }

        public IUnitOfWorkAsync NewUnitOfWorkAsync()
        {
            return new UnitOfWork((IDataContext)_context);
        }

        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlRaw(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlCommandAsync(string sql, CancellationToken cancellationToken, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken, parameters);
        }

        public virtual bool Commit()
        {
            _context.SaveChanges();
            return true;
        }

        public virtual void Rollback()
        {
            Transaction.Rollback();
            _context.Dispose();
        }

        Task<int> IUnitOfWorkAsync.SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }


        Task<int> IUnitOfWorkAsync.ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

    }
}
