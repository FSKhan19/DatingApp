using DatingApp.Backend.Consts;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Models.User
{
    public class RegisterUserInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
