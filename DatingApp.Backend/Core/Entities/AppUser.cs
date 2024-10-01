using DatingApp.Backend.Core.Auditing;
using DatingApp.Backend.Extensions;

namespace DatingApp.Backend.Core.Entities
{
    public class AppUser: FullAuditedEntity<int>
    {
        public required string UserName { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime LastActive { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set;}
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }

        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}
