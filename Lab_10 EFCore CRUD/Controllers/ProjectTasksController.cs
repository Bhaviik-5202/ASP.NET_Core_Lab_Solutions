using Lab_10_EFCore_CRUD.Data;
using Lab_10_EFCore_CRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_10_EFCore_CRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjectTasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _context.ProjectTasks
                .Include(t => t.Project)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetAllTasksPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? status = null)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var query = _context.ProjectTasks.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(t => t.Status.ToLower() == status.ToLower());
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(t => t.ProjectTaskId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                items,
                pageNumber,
                pageSize,
                totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProjectTask task)
        {
            _context.ProjectTasks.Add(task);

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProjectTask task)
        {
            if (id != task.ProjectTaskId)
                return BadRequest();

            var oldTask = await _context.ProjectTasks.FindAsync(id);

            if (oldTask == null)
                return NotFound();

            oldTask.TaskName = task.TaskName;
            oldTask.Description = task.Description;
            oldTask.Status = task.Status;
            oldTask.ProjectId = task.ProjectId;

            await _context.SaveChangesAsync();

            return Ok(oldTask);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);

            if (task == null)
                return NotFound();

            _context.ProjectTasks.Remove(task);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
