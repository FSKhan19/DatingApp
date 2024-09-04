namespace DatingApp.Backend.Core.Auditing.Interfaces
{
    public interface ICreationAudited : IHasCreationTime
    {
        long? CreatorUserId { get; set; }
    }
}
