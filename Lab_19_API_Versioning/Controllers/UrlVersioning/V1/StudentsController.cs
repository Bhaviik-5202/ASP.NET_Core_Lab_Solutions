using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Lab_19_API_Versioning.Controllers.UrlVersioning.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUser(int id = 1)
        {
            var studentV1 = new
            {
                apiVersion = "1.0",
                strategy = "URL Path Versioning (/api/v1/Students/GetUser)",
                id = id,
                full_name = "Bhavik Parmar",
                contact_email = "bhavik@example.com"
            };

            return Ok(studentV1);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = new[]
            {
                new { id = 1, full_name = "Bhavik Parmar", contact_email = "bhavik@example.com" },
                new { id = 2, full_name = "Rahul Sharma", contact_email = "rahul@example.com" }
            };

            return Ok(new { apiVersion = "1.0", count = list.Length, data = list });
        }
    }
}
