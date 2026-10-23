using FluentValidation;
using Lab_20_21_Validation.DTOs;

namespace Lab_20_21_Validation.Validators
{
    public class CourseValidator : AbstractValidator<CourseDTO>
    {
        public CourseValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("Course ID must be greater than 0");

            RuleFor(x => x.CourseName)
                .NotEmpty()
                .WithMessage("Course Name is required")
                .MaximumLength(100)
                .WithMessage("Course Name cannot exceed 100 characters");

            RuleFor(x => x.Credits)
                .InclusiveBetween(1, 6)
                .WithMessage("Credits must be between 1 and 6");
        }
    }
}
