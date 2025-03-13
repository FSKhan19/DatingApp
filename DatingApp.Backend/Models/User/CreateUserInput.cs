using DatingApp.Backend.Consts;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Models.User
{
    public class CreateUserInput
    {
        public string UserName { get; set; } = string.Empty;
    }
}
