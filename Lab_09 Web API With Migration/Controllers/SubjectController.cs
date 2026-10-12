using Lab_09_Web_API_With_Migration.Data;
using Lab_09_Web_API_With_Migration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_09_Web_API_With_Migration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SubjectController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetSubjects()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return Ok(subjects);    
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return Ok(subject);
        }

        [HttpPost]
        public async Task<ActionResult<Subject>> CreateSubject(Subject subject)
        {
            subject.Id = 0;
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSubject(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return BadRequest();
            }
            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(subject);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubject(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return Ok(subject);
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
