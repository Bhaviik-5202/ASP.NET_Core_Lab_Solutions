using Lab_10_EFCore_CRUD.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab_10_EFCore_CRUD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("total-students")]
        public async Task<IActionResult> GetTotalStudents()
        {
            var count = await _context.Users
                .Include(u => u.Role)
                .CountAsync(u => u.Role != null && u.Role.RoleName.ToLower() == "student");

            return Ok(new { TotalStudents = count });
        }

        [HttpGet("total-faculty")]
        public async Task<IActionResult> GetTotalFaculty()
        {
            var count = await _context.Users
                .Include(u => u.Role)
                .CountAsync(u => u.Role != null && u.Role.RoleName.ToLower() == "faculty");

            return Ok(new { TotalFaculty = count });
        }

        [HttpGet("total-projects")]
        public async Task<IActionResult> GetTotalProjects()
        {
            var count = await _context.Projects.CountAsync();
            return Ok(new { TotalProjects = count });
        }

        [HttpGet("total-tasks")]
        public async Task<IActionResult> GetTotalTasks()
        {
            var count = await _context.ProjectTasks.CountAsync();
            return Ok(new { TotalTasks = count });
        }

        [HttpGet("task-status-summary")]
        public async Task<IActionResult> GetTaskStatusSummary()
        {
            var summary = await _context.ProjectTasks
                .GroupBy(t => string.IsNullOrWhiteSpace(t.Status) ? "Pending" : t.Status)
                .Select(g => new
                {
                    Status = g.Key,
                    TotalTasks = g.Count()
                })
                .ToListAsync();

            return Ok(summary);
        }

        [HttpGet("faculty-project-allocations")]
        public async Task<IActionResult> GetFacultyAllocations()
        {
            var workload = await _context.ProjectAllocations
                .Include(pa => pa.User)
                .GroupBy(pa => pa.User != null ? pa.User.Name : "Unassigned")
                .Select(g => new
                {
                    FacultyName = g.Key,
                    TotalProjects = g.Count()
                })
                .OrderByDescending(x => x.TotalProjects)
                .ToListAsync();

            return Ok(workload);
        }

        [HttpGet("role-user-count")]
        public async Task<IActionResult> GetRoleWiseUserCount()
        {
            var result = await _context.Users
                .Include(u => u.Role)
                .GroupBy(u => u.Role != null ? u.Role.RoleName : "No Role")
                .Select(g => new
                {
                    Role = g.Key,
                    TotalUsers = g.Count()
                })
                .OrderByDescending(x => x.TotalUsers)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("roles-with-users")]
        public async Task<IActionResult> GetRolesWithUsers()
        {
            var result = await _context.Roles
                .Include(r => r.Users)
                .Select(r => new
                {
                    RoleName = r.RoleName,
                    Users = r.Users.Select(u => u.Name).ToList()
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("roles-with-min-users")]
        public async Task<IActionResult> GetRolesWithMinUsers([FromQuery] int minUsers = 1)
        {
            var result = await _context.Roles
                .Include(r => r.Users)
                .Where(r => r.Users.Count >= minUsers)
                .Select(r => new
                {
                    RoleName = r.RoleName,
                    TotalUsers = r.Users.Count
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalProjects = await _context.Projects.CountAsync();
            var totalTasks = await _context.ProjectTasks.CountAsync();
            var taskStatus = await _context.ProjectTasks
                .GroupBy(t => string.IsNullOrWhiteSpace(t.Status) ? "Pending" : t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            return Ok(new
            {
                TotalUsers = totalUsers,
                TotalProjects = totalProjects,
                TotalTasks = totalTasks,
                TasksByStatus = taskStatus
            });
        }
    }
}
