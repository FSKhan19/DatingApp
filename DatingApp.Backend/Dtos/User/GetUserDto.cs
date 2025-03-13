using DatingApp.Backend.Dtos.Auditing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DatingApp.Backend.Dtos.User
{
    public class GetUserDto : AuditedEntityDto<int>
    {
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
