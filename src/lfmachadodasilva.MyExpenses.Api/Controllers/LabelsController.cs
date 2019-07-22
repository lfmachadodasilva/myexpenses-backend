using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private static ICollection<LabelDto> _labels;
        public LabelsController()
        {
            if(_labels == null)
            {
                Random rnd = new Random();
                _labels = new List<LabelDto>();
                for (int i = 0; i < 20; i++)
                {
                    _labels.Add(new LabelDto
                    {
                        Id = i,
                        Name = $"LabelName{i}",
                        CurrentValue = rnd.Next(1, 250),
                        AverageValue = rnd.Next(1, 250),
                        LastValue = rnd.Next(1, 250)
                    });
                }
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
            value.Id = _labels.Count();

            Random rnd = new Random();
            value.CurrentValue = rnd.Next(1, 250);
            value.AverageValue = rnd.Next(1, 250);
            value.LastValue = rnd.Next(1, 250);

            _labels.Add(value);
            return Ok(value);
        }

        // PUT api/values/5
        [HttpPut()]
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
