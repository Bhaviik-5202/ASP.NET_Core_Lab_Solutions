using Lab_08_StudentSubjectAPI_With_HTTP_Status_Codes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab_08_StudentSubjectAPI_With_HTTP_Status_Codes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private static readonly List<Student> Students = new List<Student>
        {
            new Student { Id = 1, Name = "Bhavik", SubjectId = 1 },
            new Student { Id = 2, Name = "Ronak", SubjectId = 2 }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Students);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = Students.FirstOrDefault(x => x.Id == id);

            if (student == null)
                return NotFound("Student not found.");

            return Ok(student);
        }

        [HttpPost]
        public IActionResult Add(Student student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Name))
                return BadRequest("Invalid student data.");

            if (student.Id <= 0)
            {
                student.Id = Students.Any() ? Students.Max(x => x.Id) + 1 : 1;
            }
            else if (Students.Any(x => x.Id == student.Id))
            {
                return Conflict("Student ID already exists.");
            }

            Students.Add(student);

            return CreatedAtAction(
                nameof(GetById),
                new { id = student.Id },
                student
            );
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Student updatedStudent)
        {
            var student = Students.FirstOrDefault(x => x.Id == id);

            if (student == null)
                return NotFound("Student not found.");

            if (updatedStudent == null || string.IsNullOrWhiteSpace(updatedStudent.Name))
                return BadRequest("Invalid student data.");

            student.Name = updatedStudent.Name;
            student.SubjectId = updatedStudent.SubjectId;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var student = Students.FirstOrDefault(x => x.Id == id);

            if (student == null)
                return NotFound("Student not found.");

            Students.Remove(student);

            return NoContent();
        }
    }
}
