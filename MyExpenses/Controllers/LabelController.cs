using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public ExpenseController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        // GET api/label
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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
