using Lab_08_StudentSubjectAPI_With_HTTP_Status_Codes.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab_08_StudentSubjectAPI_With_HTTP_Status_Codes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private static readonly List<Subject> Subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "C++" },
            new Subject { Id = 2, Name = "ASP.NET Core" }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Subjects);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var subject = Subjects.FirstOrDefault(x => x.Id == id);

            if (subject == null)
                return NotFound("Subject not found.");

            return Ok(subject);
        }

        [HttpPost]
        public IActionResult Add(Subject subject)
        {
            if (subject == null || string.IsNullOrWhiteSpace(subject.Name))
                return BadRequest("Invalid subject data.");

            if (subject.Id <= 0)
            {
                subject.Id = Subjects.Any() ? Subjects.Max(x => x.Id) + 1 : 1;
            }
            else if (Subjects.Any(x => x.Id == subject.Id))
            {
                return Conflict("Subject ID already exists.");
            }

            Subjects.Add(subject);

            return CreatedAtAction(
                nameof(GetById),
                new { id = subject.Id },
                subject
            );
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Subject updatedSubject)
        {
            var subject = Subjects.FirstOrDefault(x => x.Id == id);

            if (subject == null)
                return NotFound("Subject not found.");

            if (updatedSubject == null || string.IsNullOrWhiteSpace(updatedSubject.Name))
                return BadRequest("Invalid subject data.");

            subject.Name = updatedSubject.Name;

            return NoContent();
        } 

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var subject = Subjects.FirstOrDefault(x => x.Id == id);

            if (subject == null)
                return NotFound("Subject not found.");

            Subjects.Remove(subject);

            return NoContent();
        }
    }
}
