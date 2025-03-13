using DatingApp.Backend.Dtos.Auditing.Interfaces;

namespace DatingApp.Backend.Dtos.Auditing
{
    public abstract class CreationAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ICreationAuditedDto
    {
        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }
    }
}
