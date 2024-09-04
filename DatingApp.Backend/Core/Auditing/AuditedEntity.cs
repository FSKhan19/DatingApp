using DatingApp.Backend.Core.Auditing.Interfaces;

namespace DatingApp.Backend.Core.Auditing
{
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, IModificationAudited
    {
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual long? LastModifierUserId { get; set; }
    }
}
