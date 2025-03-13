namespace DatingApp.Backend.Dtos.Auditing.Interfaces
{
    public interface IHasModificationTimeDto
    {
        DateTime? LastModificationTime { get; set; }
    }
}
