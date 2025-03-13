using DatingApp.Backend.Core.Auditing.Interfaces;
using DatingApp.Backend.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Data.Repositories
{
    public class EfRepositoryBase<TEntity> : EfRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public EfRepositoryBase(DatingAppContext appContext)
            : base(appContext)
        {
        }
    }
}
