using DatingApp.Backend.Core.Auditing.Interfaces;
using DatingApp.Backend.Core.Repositories;
using DatingApp.Backend.Services.Interfaces;

namespace DatingApp.Backend.Core
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        void Rollback();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity<int>;
        IRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>;
    }
}
