using DatingApp.Backend.Consts;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Models.User
{
    [SwaggerSchema("Data required to create a user.")]
    public class CreateUser
    {
        [Required(AllowEmptyStrings = false)]
        [SwaggerSchema("The username of the user. This field is required.")]
        public string UserName { get; set; } = string.Empty;
    }
}
