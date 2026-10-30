using Microsoft.AspNetCore.Mvc;
using Lab_25_26_Architecture.DTOs;
using Lab_25_26_Architecture.Services;
using Lab_25_26_Architecture.Validators;

namespace Lab_25_26_Architecture.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserValidator _userValidator;

        public UserController(UserValidator userValidator, IUserService userService)
        {
            _userValidator = userValidator;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserDto? dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Request body cannot be null or empty." });
            }

            var validationResult = await _userValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var message = await _userService.AddAsync(dto);
            return Ok(new { Status = "Success", Message = message });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto? dto)
        {
            if (dto == null)
            {
                return BadRequest(new { Message = "Request body cannot be null or empty." });
            }

            var validationResult = await _userValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var updated = await _userService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            return Ok(new { Status = "Success", Message = "Record Updated Successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _userService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }

            return Ok(new { Status = "Success", Message = "Record Deleted Successfully" });
        }
    }
}
