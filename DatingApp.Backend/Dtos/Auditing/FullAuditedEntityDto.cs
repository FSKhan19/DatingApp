using DatingApp.Backend.Dtos.Auditing.Interfaces;

namespace DatingApp.Backend.Dtos.Auditing
{
    public abstract class FullAuditedEntityDto<TPrimaryKey> : AuditedEntityDto<TPrimaryKey>, IDeletionAuditedDto
    {
        public virtual bool IsDeleted { get; set; }
        public virtual long? DeleterUserId { get; set; }
        public virtual DateTime? DeletionTime { get; set; }
    }
}
