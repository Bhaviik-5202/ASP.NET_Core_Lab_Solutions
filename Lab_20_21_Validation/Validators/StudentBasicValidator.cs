using FluentValidation;
using Lab_20_21_Validation.DTOs;

namespace Lab_20_21_Validation.Validators
{
    public class StudentBasicValidator : AbstractValidator<StudentBasicDTO>
    {
        public StudentBasicValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Student Name is required")
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage("Student Name cannot be empty or whitespace")
                .Must(name => !name.Any(char.IsDigit))
                .WithMessage("Student Name cannot contain Digits")
                .MaximumLength(100)
                .WithMessage("Student Name cannot exceed 100 characters");

            RuleFor(x => x.EnrollmentNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Enrollment Number is required")
                .Must(enrollment => !string.IsNullOrWhiteSpace(enrollment))
                .WithMessage("Enrollment Number cannot be empty or whitespace")
                .Length(11)
                .WithMessage("Enrollment Number must be exactly 11 characters");

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0)
                .WithMessage("Please select a valid Department");
        }
    }
}
