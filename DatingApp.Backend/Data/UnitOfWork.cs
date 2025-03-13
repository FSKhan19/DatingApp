using DatingApp.Backend.Core;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using DatingApp.Backend.Core.Auditing.Interfaces;
using DatingApp.Backend.Core.Repositories;
using DatingApp.Backend.Exceptions;

namespace DatingApp.Backend.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatingAppContext _context;
        private IDbContextTransaction? _transaction; // Nullable for EF Core
        private readonly IServiceProvider _serviceProvider; // For resolving repositories
        private readonly Dictionary<(Type, Type), object> _repositories;

        public UnitOfWork(DatingAppContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _repositories = new Dictionary<(Type, Type), object>();
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }
            _transaction = await _context.Database.BeginTransactionAsync(); // For EF Core
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                // Ensure _transaction is not null before committing
                if (_transaction == null)
                {
                    throw new InvalidOperationException("No transaction is in progress.");
                }

                await _transaction.CommitAsync();
                _transaction.Dispose();
                _transaction = null; // Reset transaction after commit
            }
            catch (DbUpdateException dbEx)
            {
                // Handle the exception and log details
                var errorMessage = $"An error occurred while saving changes: {dbEx.Message}";

                // Optionally, inspect dbEx entries for more details
                foreach (var entry in dbEx.Entries)
                {
                    errorMessage += $" Entity: {entry.Entity}, State: {entry.State}";
                }

                Rollback(); // Ensure rollback on exception

                throw new CustomException(errorMessage, dbEx);
            }
            catch (Exception)
            {
                Rollback(); // Ensure rollback on exception
                throw; // Re-throw exception after rollback
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null; // Reset transaction after rollback
            }
        }

        public IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class, IEntity<int>
        {
            var key = (typeof(TEntity), typeof(int));

            if (_repositories.ContainsKey(key))
            {
                return (IRepository<TEntity>)_repositories[key];
            }

            var repositoryInstance = _serviceProvider.GetService<IRepository<TEntity>>();

            if (repositoryInstance == null)
            {
                throw new InvalidOperationException($"Repository for type {typeof(TEntity).Name} is not registered.");
            }

            _repositories.Add(key, repositoryInstance);
            return repositoryInstance;
        }

        public IRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>()
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var key = (typeof(TEntity), typeof(TPrimaryKey));

            if (_repositories.ContainsKey(key))
            {
                return (IRepository<TEntity, TPrimaryKey>)_repositories[key];
            }

            var repositoryInstance = _serviceProvider.GetService<IRepository<TEntity, TPrimaryKey>>();

            if (repositoryInstance == null)
            {
                throw new InvalidOperationException($"Repository for type {typeof(TEntity).Name} with primary key {typeof(TPrimaryKey).Name} is not registered.");
            }

            _repositories.Add(key, repositoryInstance);
            return repositoryInstance;
        }

        public void Dispose()
        {
            Rollback(); // Ensure rollback on dispose if needed
            _context.Dispose();
        }
    }
}