using Lab_15_17_API_Fundamentals.DTOs;
using Lab_15_17_API_Fundamentals.Models;
using Lab_15_17_API_Fundamentals.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lab_15_17_API_Fundamentals.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private static readonly List<Student> _students = new()
        {
            new Student { StudentId = 1, ExternalId = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Bhavik", EnrollmentNumber = "21001", DepartmentId = 1, DepartmentName = "Computer", Score = 95, CourseCode = "CS-101" },
            new Student { StudentId = 2, ExternalId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Rahul", EnrollmentNumber = "21002", DepartmentId = 2, DepartmentName = "IT", Score = 88, CourseCode = "IT-201" },
            new Student { StudentId = 3, ExternalId = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Amit", EnrollmentNumber = "21003", DepartmentId = 1, DepartmentName = "Computer", Score = 74, CourseCode = "CS-102" },
            new Student { StudentId = 4, ExternalId = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Neha", EnrollmentNumber = "21004", DepartmentId = 3, DepartmentName = "Mechanical", Score = 91, CourseCode = "ME-301" },
            new Student { StudentId = 5, ExternalId = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Pooja", EnrollmentNumber = "21005", DepartmentId = 2, DepartmentName = "IT", Score = 65, CourseCode = "IT-202" }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            var dtos = _students.Select(MapToDto).ToList();
            return Ok(ApiResponse<List<StudentDto>>.Ok(dtos, "Students retrieved successfully"));
        }

        [HttpGet("search")]
        public IActionResult SearchStudents([FromQuery] StudentFilterParameters filter)
        {
            var query = _students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Department))
            {
                query = query.Where(s => s.DepartmentName.Contains(filter.Department, StringComparison.OrdinalIgnoreCase));
            }

            if (filter.MinScore.HasValue)
            {
                query = query.Where(s => s.Score >= filter.MinScore.Value);
            }

            var results = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(MapToDto)
                .ToList();

            return Ok(ApiResponse<List<StudentDto>>.Ok(results, $"Found {results.Count} matching student(s)"));
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == id);
            if (student == null)
            {
                return NotFound(ApiResponse<StudentDto>.Fail($"Student with ID '{id}' was not found."));
            }

            return Ok(ApiResponse<StudentDto>.Ok(MapToDto(student), "Student found"));
        }

        [HttpGet("score/{score:range(0,100)}")]
        public IActionResult GetByScoreRange([FromRoute] int score)
        {
            var matched = _students.Where(s => s.Score >= score).Select(MapToDto).ToList();
            return Ok(ApiResponse<List<StudentDto>>.Ok(matched, $"Students with score >= {score}"));
        }

        [HttpGet("name/{name:alpha}")]
        public IActionResult GetByName([FromRoute] string name)
        {
            var matched = _students.Where(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).Select(MapToDto).ToList();
            return Ok(ApiResponse<List<StudentDto>>.Ok(matched, $"Students with name '{name}'"));
        }

        [HttpGet("guid/{id:guid}")]
        public IActionResult GetByGuid([FromRoute] Guid id)
        {
            var student = _students.FirstOrDefault(s => s.ExternalId == id);
            if (student == null)
            {
                return NotFound(ApiResponse<StudentDto>.Fail($"No student matches GUID: {id}"));
            }

            return Ok(ApiResponse<StudentDto>.Ok(MapToDto(student), "Student found by GUID"));
        }

        [HttpGet(@"code/{code:regex(^[[A-Za-z]]{{2,4}}-[[0-9]]{{3}}$)}")]
        public IActionResult GetByCourseCode([FromRoute] string code)
        {
            var matched = _students.Where(s => s.CourseCode.Equals(code, StringComparison.OrdinalIgnoreCase)).Select(MapToDto).ToList();
            return Ok(ApiResponse<List<StudentDto>>.Ok(matched, $"Students in course '{code}'"));
        }

        [HttpGet("eligible-age/{age:min(18)}")]
        public IActionResult CheckEligibility([FromRoute] int age)
        {
            return Ok(ApiResponse<object>.Ok(new { Age = age, IsEligible = true }, "Age constraint validated successfully"));
        }

        [HttpPost]
        public IActionResult CreateStudent([FromBody] CreateStudentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.EnrollmentNumber))
            {
                return BadRequest(ApiResponse<StudentDto>.Fail("Validation failed", new List<string> { "Name and Enrollment Number are required." }));
            }

            var newStudent = new Student
            {
                StudentId = _students.Max(s => s.StudentId) + 1,
                ExternalId = Guid.NewGuid(),
                Name = dto.Name,
                EnrollmentNumber = dto.EnrollmentNumber,
                DepartmentId = dto.DepartmentId,
                DepartmentName = dto.DepartmentName,
                Score = dto.Score,
                CourseCode = dto.CourseCode
            };

            _students.Add(newStudent);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newStudent.StudentId },
                ApiResponse<StudentDto>.Ok(MapToDto(newStudent), "Student created successfully")
            );
        }

        [HttpGet("header-info")]
        public IActionResult ReadHeaderInfo(
            [FromHeader(Name = "X-Client-App")] string? clientApp,
            [FromHeader(Name = "X-Request-Id")] string? requestId,
            [FromHeader(Name = "User-Agent")] string? userAgent)
        {
            var headerData = new
            {
                ClientApp = clientApp ?? "Not provided (send via 'X-Client-App' header)",
                RequestId = requestId ?? Guid.NewGuid().ToString(),
                UserAgent = userAgent ?? "Unknown"
            };

            return Ok(ApiResponse<object>.Ok(headerData, "Header values successfully bound"));
        }

        [HttpPost("submit-feedback")]
        [Consumes("multipart/form-data", "application/x-www-form-urlencoded")]
        public IActionResult SubmitFeedback([FromForm] StudentFormDto form)
        {
            if (string.IsNullOrWhiteSpace(form.FullName))
            {
                return BadRequest(ApiResponse<object>.Fail("FullName is required in form submission."));
            }

            var result = new
            {
                ReceivedFullName = form.FullName,
                ReceivedEnrollmentNo = form.EnrollmentNo,
                ReceivedNote = form.FeedbackNote,
                SubmissionTime = DateTime.UtcNow
            };

            return Ok(ApiResponse<object>.Ok(result, "Form submission processed successfully"));
        }

        private static StudentDto MapToDto(Student s) => new()
        {
            StudentId = s.StudentId,
            ExternalId = s.ExternalId,
            Name = s.Name,
            EnrollmentNumber = s.EnrollmentNumber,
            DepartmentId = s.DepartmentId,
            DepartmentName = s.DepartmentName,
            Score = s.Score,
            CourseCode = s.CourseCode
        };
    }
}
