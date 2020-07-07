using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidateHelper _validateHelper;

        public UserController(IUserService userService, IValidateHelper validateHelper)
        {
            _userService = userService;
            _validateHelper = validateHelper;
        }

        // GET api/user
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<UserModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var models = await _userService.GetAllAsync();
            return Ok(models);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post([FromBody] UserModel value)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            if (!userId.Equals(value.Id))
            {
                return Forbid();
            }

            try
            {
                var model = await _userService.AddOrUpdateAsync(value);
                return Ok(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest(e);
            }
        }
    }
}
