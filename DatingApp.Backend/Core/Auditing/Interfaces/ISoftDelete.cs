namespace DatingApp.Backend.Core.Auditing.Interfaces
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
