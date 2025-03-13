namespace DatingApp.Backend.Dtos.Auditing.Interfaces
{
    public interface IModificationAuditedDto : IHasModificationTimeDto
    {
        long? LastModifierUserId { get; set; }
    }
}
