using FluentValidation;
using Lab_25_26_Architecture.DTOs;

namespace Lab_25_26_Architecture.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Full Name is required.")
                .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

            RuleFor(x => x.MobileNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Mobile number is required.")
                .Matches(@"^[0-9]{10}$").WithMessage("Mobile number must be exactly 10 digits.");

            RuleFor(x => x.UserTypeID)
                .InclusiveBetween(1, 3).WithMessage("UserTypeID must be 1 (Admin), 2 (Faculty), or 3 (Student).");
        }
    }
}
