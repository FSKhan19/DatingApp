using DatingApp.Backend.Consts;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Models.User
{
    public class RegisterUserRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
    public class RegisterUserResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
