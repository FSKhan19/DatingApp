using DatingApp.Backend.Consts;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Models.User
{
    public class LoginUserRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = Error.DataValidation.ENTER_VALID_STRING)]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = Error.DataValidation.ENTER_VALID_STRING)]
        public string Password { get; set; }
    }

    public class LoginUserResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}
