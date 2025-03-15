using DatingApp.Backend.Dtos.Auditing;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DatingApp.Backend.Dtos.User
{
    public class UserDto : AuditedEntityDto<int>
    {
        public string UserName { get; set; } = string.Empty;
    }
}
