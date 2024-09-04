namespace DatingApp.Backend.Core.Auditing.Interfaces
{
    public interface IHasDeletionTime : ISoftDelete
    {
        DateTime? DeletionTime { get; set; }
    }
}
