using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Get()
        {
            var userId = _validateHelper.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
            var results = await _groupService.GetAllAsync(userId);
            return Ok(results);
        }

        // GET api/group/full
        [HttpGet("full")]
        [ProducesResponseType(typeof(ICollection<GroupGetFullModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFull()
        {
            var userId = _validateHelper.GetUserId(HttpContext.User.Identity as ClaimsIdentity);
            var results = await _groupService.GetAllFullAsync(userId);
            return Ok(results);
        }

        // GET api/group/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupGetFullModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok();
        }

        // POST api/group
        [HttpPost]
        [ProducesResponseType(typeof(GroupGetFullModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Post([FromBody] GroupGetFullModel value)
        {
            return Ok();
        }

        // PUT api/group
        [HttpPut]
        [ProducesResponseType(typeof(GroupGetFullModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] GroupGetFullModel value)
        {
            return Ok();

        }

        // DELETE api/group/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void Delete(int id)
        {
        }
    }
}
