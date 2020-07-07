using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IValidateHelper _validateHelper;

        public GroupController(IGroupService groupService, IValidateHelper validateHelper)
        {
            _groupService = groupService;
            _validateHelper = validateHelper;
        }

        // GET api/group
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<GroupGetModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var a = HttpContext;
            var userId = _validateHelper.GetUserId(HttpContext);
            var results = await _groupService.GetAllAsync(userId);
            return Ok(results);
        }

        // GET api/group/full
        [HttpGet("full")]
        [ProducesResponseType(typeof(ICollection<GroupGetFullModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull()
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var results = await _groupService.GetAllFullAsync(userId);
            return Ok(results);
        }

        // GET api/group/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupGetFullModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var result = await _groupService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            if (!result.Users.Any(gu => gu.Id.Equals(userId)))
            {
                return Forbid();
            }

            return Ok(result);
        }

        // POST api/group
        [HttpPost]
        [ProducesResponseType((typeof(GroupAddModel)), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] GroupAddModel value)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            if (value != null && !value.Users.Any(u => u.Id.Equals(userId)))
            {
                return Forbid();
            }

            var model = await _groupService.AddAsync(value);
            if (model == null)
            {
                return BadRequest();
            }
            return Ok(model);
        }

        // PUT api/group
        [HttpPut]
        [ProducesResponseType(typeof(GroupManageModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] GroupManageModel value)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            if (value != null && !value.Users.Any(u => u.Id.Equals(userId)))
            {
                return Forbid();
            }

            try
            {
                var model = await _groupService.UpdateAsync(value);
                if (model == null)
                {
                    return BadRequest();
                }

                return Ok(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _groupService.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            var userId = _validateHelper.GetUserId(HttpContext);
            if (!result.Users.Any(gu => gu.Id.Equals(userId)))
            {
                return Forbid();
            }

            if (!await _groupService.DeleteAsync(id))
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
