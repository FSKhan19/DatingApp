namespace DatingApp.Backend.Core.Auditing.Interfaces
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
