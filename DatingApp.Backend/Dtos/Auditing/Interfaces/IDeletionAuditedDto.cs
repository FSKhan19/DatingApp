namespace DatingApp.Backend.Dtos.Auditing.Interfaces
{
    public interface IDeletionAuditedDto : IHasDeletionTimeDto, ISoftDeleteDto
    {
        long? DeleterUserId { get; set; }
    }
}
