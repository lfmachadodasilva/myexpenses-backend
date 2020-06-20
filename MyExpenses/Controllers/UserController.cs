using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/user
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<UserModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var models = await _userService.GetAllAsync();
            return Ok(models);
        }

        // // GET api/user/5
        // [HttpGet("{id}")]
        // [ProducesResponseType(typeof(ICollection<UserModel>), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Get(string id)
        // {
        //     var model = await _userService.GetAsync(id);
        //     return Ok(model);
        // }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] UserModel value)
        {
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

        // // POST api/user
        // [HttpPost]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(ModelBaseString), StatusCodes.Status409Conflict)]
        // public async Task<ActionResult<UserModel>> Post([FromBody] UserModelToAdd value)
        // {
        //     try
        //     {
        //         var model = await _userService.AddAsync(value);
        //         return Created("post", model);
        //     }
        //     catch (DbUpdateException e)
        //     {
        //         if (e.InnerException is Npgsql.PostgresException postgresException &&
        //             postgresException.SqlState.Equals("23505"))
        //         {
        //             return Conflict(new { id = value.Id });
        //         }
        //         return BadRequest();
        //     }
        // }

        // // PUT api/user/5
        // [HttpPut("{id}")]
        // [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        // public async Task<IActionResult> Put([FromBody] UserModel value)
        // {
        //     var model = await _userService.UpdateAsync(value);
        //     return Ok(model);
        // }

        // // DELETE api/user/5
        // [HttpDelete("{id}")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // public async Task<IActionResult> Delete(string id)
        // {
        //     await _userService.DeleteAsync(id);
        //     return Ok();
        // }
    }
}
