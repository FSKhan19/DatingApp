using DatingApp.Backend.Models.User;
using FluentValidation;

namespace DatingApp.Backend.Validators.User
{
    public class UpdateUserValidator: AbstractValidator<UpdateUserInput>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50).WithMessage("Username must not exceed 50 characters.");
        }
    }
}
