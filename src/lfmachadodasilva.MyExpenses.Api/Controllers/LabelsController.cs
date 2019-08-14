using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Models.Requests;
using lfmachadodasilva.MyExpenses.Api.Properties;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly FakeDatabase _fakeDatabase;

        public LabelsController(ILabelService labelService, FakeDatabase fakeDatabase)
        {
            _labelService = labelService;
            _fakeDatabase = fakeDatabase;
        }

        // GET api/values
        [HttpGet("withValues")]
        [ProducesResponseType(typeof(LabelWithValuesDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllWithValues([FromQuery]SearchRequest request)
        {
            var task = Task.Run(() =>
            {
                return _fakeDatabase.Labels.Where(x => x.GroupId.Equals(request.GroupId));
            });
            return Ok(await task);
        }

        [HttpGet]
        [ProducesResponseType(typeof(LabelWithValuesDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery][Required] long groupId)
        {
            var task = Task.Run(() =>
            {
                return _fakeDatabase.Labels.Where(x => x.GroupId.Equals(groupId));
            });
            return Ok(await task);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var task = Task.Run(() =>
            {
                return _fakeDatabase.Labels.FirstOrDefault(x => x.Id.Equals(id));

            });
            return Ok(await task);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] LabelDto value)
        {
            if(value.Name == "duplicate")
            {
                return Conflict(Resource.ErrorDuplicate);
            }
            var task = Task.Run(() =>
            {
                value.Id = _fakeDatabase.Labels.Count();

                Random rnd = new Random();
                var withValues = new LabelWithValuesDto
                {
                    Id = _fakeDatabase.Labels.Count(),
                    Name = value.Name,
                    GroupId = value.GroupId,
                    CurrentValue = rnd.Next(1, 250),
                    AverageValue = rnd.Next(1, 250),
                    LastValue = rnd.Next(1, 250),
                };

                _fakeDatabase.Labels.Add(withValues);

                return withValues;
            });
                
            return Ok(await task);
        }

        // PUT api/values/5
        [HttpPut()]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] LabelDto value)
        {
            if (value.Name == "duplicate")
            {
                return Conflict(Resource.ErrorDuplicate);
            }

            var task = Task.Run(() =>
            {
                var label = _fakeDatabase.Labels.FirstOrDefault(x => x.Id.Equals(value.Id));
                label.Name = value.Name;

                return label;
            });
                
            return Ok(await task);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task Delete(int id)
        {
            var task = Task.Run(() =>
            {
                var label = _fakeDatabase.Labels.FirstOrDefault(x => x.Id.Equals(id));
                _fakeDatabase.Labels.Remove(label);
            });
            await task;
        }
    }
}
