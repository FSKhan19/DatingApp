namespace DatingApp.Backend.Core.Auditing.Interfaces
{
    public interface IModificationAudited : IHasModificationTime
    {
        long? LastModifierUserId { get; set; }
    }
}
