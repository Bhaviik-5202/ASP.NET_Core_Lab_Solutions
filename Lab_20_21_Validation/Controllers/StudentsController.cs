using FluentValidation;
using Lab_20_21_Validation.DTOs;
using Lab_20_21_Validation.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Lab_20_21_Validation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IValidator<StudentBasicDTO> _basicValidator;
        private readonly IValidator<StudentAdvancedDTO> _advancedValidator;

        public StudentsController(
            IValidator<StudentBasicDTO> basicValidator,
            IValidator<StudentAdvancedDTO> advancedValidator)
        {
            _basicValidator = basicValidator;
            _advancedValidator = advancedValidator;
        }

        [HttpPost("basic")]
        public async Task<IActionResult> CreateBasic([FromBody] StudentBasicDTO? dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    Status = "Validation Failed",
                    TotalErrors = 1,
                    Errors = new[] { new { Property = "RequestBody", ErrorMessage = "Request body cannot be null or empty." } }
                });
            }

            var validationResult = await _basicValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                });

                return BadRequest(new
                {
                    Status = "Validation Failed",
                    TotalErrors = validationResult.Errors.Count,
                    Errors = errors
                });
            }

            return Ok(new
            {
                Status = "Validation Passed",
                Message = "Student created successfully.",
                Data = dto
            });
        }

        [HttpPost("advanced")]
        public async Task<IActionResult> CreateAdvanced([FromBody] StudentAdvancedDTO? dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    Status = "Validation Failed",
                    TotalErrors = 1,
                    Errors = new[] { new { Property = "RequestBody", ErrorMessage = "Request body cannot be null or empty." } }
                });
            }

            var validationResult = await _advancedValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                });

                return BadRequest(new
                {
                    Status = "Validation Failed",
                    TotalErrors = validationResult.Errors.Count,
                    Errors = errors
                });
            }

            return Ok(new
            {
                Status = "Validation Passed",
                Message = "Advanced student record created successfully.",
                Data = dto
            });
        }

        [HttpGet("sample-basic")]
        public IActionResult GetSampleBasic()
        {
            return Ok(new StudentBasicDTO
            {
                StudentId = 1,
                Name = "Bhavik Parmar",
                EnrollmentNumber = "21001234567",
                DepartmentId = 1,
                DepartmentName = "Computer Engineering"
            });
        }

        [HttpGet("sample-advanced")]
        public IActionResult GetSampleAdvanced()
        {
            return Ok(new StudentAdvancedDTO
            {
                StudentId = 101,
                EnrollmentNo = "21001234567",
                StudentName = "Bhavik Parmar",
                Age = 20,
                Gender = Gender.Male,
                DepartmentId = 1,
                Semester = 5,
                IsHosteller = true,
                HostelName = "Swami Vivekananda Hostel",
                Fees = 45000,
                Courses = new List<CourseDTO>
                {
                    new CourseDTO { CourseId = 501, CourseName = "ASP.NET Core Web API", Credits = 4 },
                    new CourseDTO { CourseId = 502, CourseName = "Cloud Computing", Credits = 3 }
                }
            });
        }
    }
}
