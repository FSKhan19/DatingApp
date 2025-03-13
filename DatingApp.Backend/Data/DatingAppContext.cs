using DatingApp.Backend.Core.Auditing.Interfaces;
using DatingApp.Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace DatingApp.Backend.Data
{
    public class DatingAppContext: DbContext
    {
        public DatingAppContext(DbContextOptions<DatingAppContext> dbContextOptions): base(dbContextOptions)
        {
                
        }

        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Apply global filter for all ISoftDelete entities
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                    var condition = Expression.Lambda(Expression.Equal(property, Expression.Constant(false)), parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(condition);
                }
            }
        }
    }
}
