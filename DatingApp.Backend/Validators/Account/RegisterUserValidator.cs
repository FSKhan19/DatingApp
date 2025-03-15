using DatingApp.Backend.Models.User;
using FluentValidation;

namespace DatingApp.Backend.Validators.Account
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserInput>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(255).WithMessage("Password must not exceed 255 characters.")
                .Matches(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]).*$")
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }
    }
}
