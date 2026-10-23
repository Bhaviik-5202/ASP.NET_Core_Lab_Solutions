using FluentValidation;
using Lab_20_21_Validation.DTOs;

namespace Lab_20_21_Validation.Validators
{
    public class StudentAdvancedValidator : AbstractValidator<StudentAdvancedDTO>
    {
        public StudentAdvancedValidator()
        {
            RuleFor(x => x.StudentName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Student Name is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.StudentName)
                        .Must(name => !name.Any(char.IsDigit)).WithMessage("Student Name cannot contain numbers.")
                        .MaximumLength(100).WithMessage("Student Name cannot exceed 100 characters.");
                });

            RuleFor(x => x.EnrollmentNo)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Enrollment Number is required.")
                .Length(11).WithMessage("Enrollment Number must be exactly 11 characters.");

            RuleFor(x => x.Gender)
                .IsInEnum().WithMessage("Please select a valid Gender (1 = Male, 2 = Female, 3 = Other).");

            RuleFor(x => x.HostelName)
                .NotEmpty().WithMessage("Hostel Name is required when student is a hosteller.")
                .When(x => x.IsHosteller);

            RuleFor(x => x.HostelName)
                .Empty().WithMessage("Hostel Name must be blank if the student is not a hosteller.")
                .Unless(x => x.IsHosteller);

            RuleFor(x => x.Fees)
                .Must(fees => fees >= 10000 && fees <= 200000)
                .WithMessage("Fees must be between ₹10,000 and ₹2,00,000.");

            RuleFor(x => x.Courses)
                .NotEmpty().WithMessage("At least one course must be registered.");

            RuleForEach(x => x.Courses)
                .SetValidator(new CourseValidator());

            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("Valid DepartmentId is required.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Age)
                        .InclusiveBetween(16, 40).WithMessage("Age must be between 16 and 40.");

                    RuleFor(x => x.Semester)
                        .InclusiveBetween(1, 8).WithMessage("Semester must be between 1 and 8.");
                });
        }
    }
}
