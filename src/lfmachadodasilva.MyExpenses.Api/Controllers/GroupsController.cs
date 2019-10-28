using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IWebSettings _webSettings;

        public GroupsController(
            IGroupService groupService,
            IWebSettings webSettings)
        {
            _groupService = groupService;
            _webSettings = webSettings;
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(GroupWithValuesDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _groupService.GetAllAsync(_webSettings.DefaultUserId));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GroupWithValuesDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _groupService.GetByIdAsync(id));
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] GroupDto value)
        {
            //    if (value.Name == "duplicate")
            //    {
            //        return Conflict(Resource.ErrorDuplicate);
            //    }
            return Ok(await _groupService.AddAsync(value));
        }

        // PUT api/values/5
        [HttpPut]
        [ProducesResponseType(typeof(GroupDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] GroupDto value)
        {
            // if (value.Name == "duplicate")
            // {
            //     return Conflict(Resource.ErrorDuplicate);
            // }

            return Ok(await _groupService.UpdateAsync(value));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<bool> Delete(int id)
        {
            return await _groupService.RemoveAsync(id);
        }
    }
}
