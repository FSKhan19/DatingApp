using DatingApp.Backend.Models.Common;
using FluentValidation;

namespace DatingApp.Backend.Validators.Common
{
    public class PaginationQueryValidator : AbstractValidator<PaginationQuery>
    {
        public PaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1) // Page number must be at least 1
                .WithMessage("Page number must be greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1) // Page size must be at least 1
                .WithMessage("Page size must be greater than or equal to 1.")
                .LessThanOrEqualTo(100) // Optional: Enforce a maximum page size (e.g., 100)
                .WithMessage("Page size cannot exceed 100.");
        }
    }
}
