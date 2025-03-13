namespace DatingApp.Backend.Dtos.Auditing.Interfaces
{
    public interface ICreationAuditedDto : IHasCreationTimeDto
    {
        long? CreatorUserId { get; set; }
    }
}
