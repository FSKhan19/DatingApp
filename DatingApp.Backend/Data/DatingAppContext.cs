using DatingApp.Backend.Core.Auditing.Interfaces;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace DatingApp.Backend.Data
{
    public class DatingAppContext : DbContext
    {
        public DatingAppContext(DbContextOptions<DatingAppContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<AppUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply global query filter for ISoftDelete entities
            modelBuilder.ApplyQueryFilter<ISoftDelete>(e => !e.IsDeleted);

            // Configure navigation properties as optional
            //modelBuilder.Entity<Photo>()
            //    .HasOne(p => p.AppUser)
            //    .WithMany(u => u.Photos)
            //    .HasForeignKey(p => p.AppUserId)
            //    .IsRequired(false);
        }
    }
}
