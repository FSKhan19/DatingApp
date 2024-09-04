namespace DatingApp.Backend.Core.Auditing
{
    public abstract class Entity<TPrimaryKey>
    {
        public TPrimaryKey Id { get; set; }
    }
}
