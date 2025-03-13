namespace DatingApp.Backend.Dtos.Auditing.Interfaces
{
    public interface ISoftDeleteDto
    {
        bool IsDeleted { get; set; }
    }
}
