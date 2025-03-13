namespace DatingApp.Backend.Dtos.Auditing.Interfaces
{
    public interface IEntityDto<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
