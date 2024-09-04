using DatingApp.Backend.Core.Auditing.Interfaces;

namespace DatingApp.Backend.Core.Auditing
{
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IDeletionAudited
    {
        public virtual bool IsDeleted { get; set; }
        public virtual long? DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
    }
}
