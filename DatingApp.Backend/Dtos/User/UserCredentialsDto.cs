using DatingApp.Backend.Dtos.Auditing;

namespace DatingApp.Backend.Dtos.User
{
    public class UserCredentialsDto: EntityDto<int>
    {
        public required string UserName { get; set; }
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}
