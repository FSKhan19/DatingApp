namespace DatingApp.Backend.Dtos.Auditing.Interfaces
{
    public interface IHasDeletionTimeDto : ISoftDeleteDto
    {
        DateTime? DeletionTime { get; set; }
    }
}
