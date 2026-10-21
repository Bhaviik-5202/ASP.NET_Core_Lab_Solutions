using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Lab_19_API_Versioning.Controllers.HeaderVersioning
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetV1()
        {
            var userV1 = new
            {
                apiVersion = "1.0",
                strategy = "Header Versioning (Header: 'X-Version: 1.0')",
                id = 101,
                full_name = "Jane Doe",
                contact_email = "jane@example.com"
            };

            return Ok(userV1);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetV2()
        {
            var userV2 = new
            {
                apiVersion = "2.0",
                strategy = "Header Versioning (Header: 'X-Version: 2.0')",
                id = "usr_101",
                name = new
                {
                    first_name = "Jane",
                    last_name = "Doe"
                },
                email = "jane@example.com",
                role = "Administrator",
                lastLogin = DateTime.UtcNow
            };

            return Ok(userV2);
        }
    }
}
