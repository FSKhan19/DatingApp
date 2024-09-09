using DatingApp.Backend.Consts;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Models.User
{
    public class RegisterUser
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = Error.DataValidation.ENTER_VALID_STRING)]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = Error.DataValidation.ENTER_VALID_STRING)]
        public string Password { get; set; }
    }
}
