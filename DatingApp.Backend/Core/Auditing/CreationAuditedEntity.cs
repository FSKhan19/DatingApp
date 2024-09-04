using DatingApp.Backend.Core.Auditing.Interfaces;

namespace DatingApp.Backend.Core.Auditing
{
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited
    {
        public virtual DateTime CreationTime { get; set; } = DateTime.Now;
        public virtual long? CreatorUserId { get; set; }
    }
}
