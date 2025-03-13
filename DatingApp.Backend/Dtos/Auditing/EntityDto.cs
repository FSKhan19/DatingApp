using DatingApp.Backend.Dtos.Auditing.Interfaces;

namespace DatingApp.Backend.Dtos.Auditing
{
    public abstract class EntityDto<TPrimaryKey>: IEntityDto<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}
