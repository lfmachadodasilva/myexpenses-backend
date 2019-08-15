using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Models.Requests;
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

        public LabelsController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        // GET api/values
        [HttpGet("withValues")]
        [ProducesResponseType(typeof(LabelWithValuesDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllWithValues([FromQuery]SearchRequest request)
        {
            var labels = await _labelService.GetAllWithValues(request.GroupId, request.Month, request.Year);
            return Ok(labels);
        }

        [HttpGet]
        [ProducesResponseType(typeof(LabelWithValuesDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery][Required] long groupId)
        {
            var labels = await _labelService.GetAll(groupId);
            return Ok(labels);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var label = await _labelService.GetByIdAsync(id);
            return Ok(label);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] LabelDto value)
        {
            // TODO
            //if(value.Name == "duplicate")
            //{
            //    return Conflict(Resource.ErrorDuplicate);
            //}

            var label = await _labelService.AddAsync(value);
            return Ok(label);
        }

        // PUT api/values/5
        [HttpPut()]
        [ProducesResponseType(typeof(LabelDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] LabelDto value)
        {
            // TODO
            //if (value.Name == "duplicate")
            //{
            //    return Conflict(Resource.ErrorDuplicate);
            //}

            var label = await _labelService.UpdateAsync(value);
            return Ok(label);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task Delete(int id)
        {
            await _labelService.RemoveAsync(id);
        }
    }
}
