using DatingApp.Backend.Core.Auditing;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DatingApp.Backend.Models.User
{
    [SwaggerSchema("Represents a user in the system.")]
    public class GetUser: AuditedEntity<int>
    {
        [SwaggerSchema("The username of the user.")]
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
