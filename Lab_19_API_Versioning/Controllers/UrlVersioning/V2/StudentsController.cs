using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Lab_19_API_Versioning.Controllers.UrlVersioning.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUser(string id = "101")
        {
            var studentV2 = new
            {
                apiVersion = "2.0",
                strategy = "URL Path Versioning (/api/v2/Students/GetUser)",
                id = $"std_{id}",
                name = new
                {
                    first_name = "Bhavik",
                    last_name = "Parmar"
                },
                email = "bhavik@example.com",
                department = "Computer Engineering",
                semester = 5,
                isActive = true
            };

            return Ok(studentV2);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = new[]
            {
                new { id = "std_101", name = new { first_name = "Bhavik", last_name = "Parmar" }, email = "bhavik@example.com", department = "CE" },
                new { id = "std_102", name = new { first_name = "Rahul", last_name = "Sharma" }, email = "rahul@example.com", department = "IT" }
            };

            return Ok(new { apiVersion = "2.0", count = list.Length, data = list });
        }
    }
}
