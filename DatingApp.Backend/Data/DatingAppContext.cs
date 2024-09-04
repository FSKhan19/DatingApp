using DatingApp.Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Backend.Data
{
    public class DatingAppContext: DbContext
    {
        public DatingAppContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
                
        }

        public DbSet<AppUser> Users { get; set; }
    }
}
