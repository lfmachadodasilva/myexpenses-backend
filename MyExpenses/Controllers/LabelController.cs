using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        // GET api/label
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            // var labels = await _labelService.GetAll();
            return Ok(await _labelService.GetAll());
        }

        // GET api/label/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/label
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/label/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/label/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
