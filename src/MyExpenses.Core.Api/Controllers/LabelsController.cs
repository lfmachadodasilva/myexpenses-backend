using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Core.Api.Models;

namespace MyExpenses.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ICollection<LabelDto> _labels = new List<LabelDto>();
        public LabelsController()
        {
            for (int i = 0; i < 20; i++)
            {
                _labels.Add(new LabelDto
                {
                    Id = i,
                    Name = $"LabelName{i}"
                });
            }
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(LabelDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            return Ok(_labels);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(_labels.FirstOrDefault(x => x.Id.Equals(id)));
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] LabelDto value)
        {
            _labels.Add(value);
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Put([FromBody] LabelDto value)
        {
            var label = _labels.FirstOrDefault(x => x.Id.Equals(value.Id));
            label.Name = value.Name;
            return Ok(label);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task Delete(int id)
        {
            var label = _labels.FirstOrDefault(x => x.Id.Equals(id));
            _labels.Remove(label);
        }
    }
}
