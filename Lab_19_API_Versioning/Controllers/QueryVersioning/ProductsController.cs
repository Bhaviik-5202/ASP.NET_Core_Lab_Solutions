using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Lab_19_API_Versioning.Controllers.QueryVersioning
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IActionResult GetV1()
        {
            var productV1 = new
            {
                apiVersion = "1.0",
                strategy = "Query String Versioning (?api-version=1.0)",
                data = new
                {
                    productId = 1,
                    name = "Mechanical Gaming Keyboard",
                    price = 2499
                }
            };

            return Ok(productV1);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public IActionResult GetV2()
        {
            var productV2 = new
            {
                apiVersion = "2.0",
                strategy = "Query String Versioning (?api-version=2.0)",
                data = new
                {
                    productId = 1,
                    name = "Mechanical Gaming Keyboard RGB",
                    price = 2499.00,
                    currency = "INR",
                    category = "Peripherals",
                    inStock = true,
                    rating = 4.8
                }
            };

            return Ok(productV2);
        }
    }
}
