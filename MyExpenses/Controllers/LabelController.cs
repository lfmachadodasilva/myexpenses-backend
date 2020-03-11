using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Models;
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

        // GET api/expense
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // var labels = await _labelService.GetAll();
            return Ok(await _labelService.GetAll());
        }

        // GET api/expense/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/expense
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/expense/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/expense/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
