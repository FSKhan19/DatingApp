using DatingApp.Backend.Core.Auditing;

namespace DatingApp.Backend.Core.Entities
{
    public class AppUser: FullAuditedEntity<int>
    {
        public string UserName { get; set; }
    }
}
