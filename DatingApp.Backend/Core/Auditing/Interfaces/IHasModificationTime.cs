namespace DatingApp.Backend.Core.Auditing.Interfaces
{
    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}
