using DatingApp.Backend.Models.Common;
using DatingApp.Backend.Models.User;
using FluentValidation;

namespace DatingApp.Backend.Validators.Common
{
    public class IdRequestValidator : AbstractValidator<IdRequest>
    {
        public IdRequestValidator()
        {
            RuleFor(x => x.id)
                .NotNull().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
