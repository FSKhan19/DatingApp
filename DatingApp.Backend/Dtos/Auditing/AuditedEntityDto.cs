using DatingApp.Backend.Dtos.Auditing.Interfaces;

namespace DatingApp.Backend.Dtos.Auditing
{
    public abstract class AuditedEntityDto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, IModificationAuditedDto
    {
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual long? LastModifierUserId { get; set; }
    }
}
