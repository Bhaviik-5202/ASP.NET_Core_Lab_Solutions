using Lab_07_Student_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab_07_Student_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectsController : ControllerBase
    {
        private static List<Subject> subjects = new List<Subject>()
        {
            new Subject{Id=1,Name="Mathematics",Credits=4},
            new Subject{Id=2,Name="Physics",Credits=3},
            new Subject{Id=3,Name="Computer Science",Credits=5},
            new Subject{Id=4,Name="Database",Credits=4},
            new Subject{Id=5,Name="Operating System",Credits=4}
        };

        [HttpGet]
        public IActionResult GetSubjects()
        {
            return Ok(subjects);
        }

        [HttpGet("{id}")]
        public IActionResult GetSubject(int id)
        {
            var subject = subjects.FirstOrDefault(x => x.Id == id);

            if (subject == null)
                return NotFound("Subject Not Found");

            return Ok(subject);
        }

        [HttpPost]
        public IActionResult AddSubject(Subject subject)
        {
            if (subject == null)
                return BadRequest("Invalid Subject Data");

            if (subject.Id <= 0)
            {
                subject.Id = subjects.Any() ? subjects.Max(x => x.Id) + 1 : 1;
            }
            else if (subjects.Any(x => x.Id == subject.Id))
            {
                return Conflict("Subject with this ID already exists");
            }

            subjects.Add(subject);

            return CreatedAtAction(
                nameof(GetSubject),
                new { id = subject.Id },
                subject
            );
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSubject(int id, Subject updatedSubject)
        {
            if (updatedSubject == null)
                return BadRequest("Invalid Subject Data");

            var subject = subjects.FirstOrDefault(x => x.Id == id);

            if (subject == null)
                return NotFound("Subject Not Found");

            subject.Name = updatedSubject.Name;
            subject.Credits = updatedSubject.Credits;

            return Ok("Subject Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSubject(int id)
        {
            var subject = subjects.FirstOrDefault(x => x.Id == id);

            if (subject == null)
                return NotFound("Subject Not Found");

            subjects.Remove(subject);

            return Ok("Subject Deleted Successfully");
        }
    }
}