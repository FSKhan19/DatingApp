using DatingApp.Backend.Consts;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Models.User
{
    [SwaggerSchema("Data required to update a user.")]
    public class UpdateUser
    {
        [Required(AllowEmptyStrings = false)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [SwaggerSchema("The username of the user. This field is required.")]
        public string UserName { get; set; } = string.Empty;
    }
}
