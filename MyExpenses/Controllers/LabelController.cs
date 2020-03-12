using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Models;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/group/{group}/labels")]
    [ApiController]
    [Authorize]
    public class LabelController : MyControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelController(ILabelService labelService, IGroupService groupService)
            : base(groupService)
        {
            _labelService = labelService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(LabelModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(long group)
        {
            if (!await Validate(group))
            {
                return Forbid();
            }

            return Ok(await _labelService.GetAll());
        }

        [HttpGet("detailed")]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(LabelModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetWithDetails(long group)
        {
            if (!await Validate(group))
            {
                return Unauthorized("USER_UNAUTHORIZED_BY_GROUP");
            }

            return Ok(await _labelService.GetAll());
        }

        // GET api/label/5
        [HttpGet("{id}")]
        public string Get(long group, long id)
        {
            return "value";
        }

        // POST api/label
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/label/5
        [HttpPut]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/label/5
        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
