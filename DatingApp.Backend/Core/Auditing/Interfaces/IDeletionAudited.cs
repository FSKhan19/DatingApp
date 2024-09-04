namespace DatingApp.Backend.Core.Auditing.Interfaces
{
    public interface IDeletionAudited : IHasDeletionTime, ISoftDelete
    {
        long? DeleterUserId { get; set; }
    }
}
