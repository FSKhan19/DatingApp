using DatingApp.Backend.Core.Auditing.Interfaces;

namespace DatingApp.Backend.Core.Auditing
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}
