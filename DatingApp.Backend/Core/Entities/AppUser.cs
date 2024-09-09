using DatingApp.Backend.Core.Auditing;

namespace DatingApp.Backend.Core.Entities
{
    public class AppUser: FullAuditedEntity<int>
    {
        public required string UserName { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}
