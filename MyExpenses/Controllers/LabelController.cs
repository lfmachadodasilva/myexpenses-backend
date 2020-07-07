using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.Services;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IValidateHelper _validateHelper;

        public LabelController(ILabelService labelService, IValidateHelper validateHelper)
        {
            _labelService = labelService;
            _validateHelper = validateHelper;
        }

        // GET api/label
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<LabelManageModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(long group)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var results = await _labelService.GetAllAsync(userId, group);
            return Ok(results);
        }

        // GET api/label/full
        [HttpGet("full")]
        [ProducesResponseType(typeof(ICollection<LabelGetFullModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull(long group, int month, int year)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var results = await _labelService.GetAllFullAsync(userId, group, month, year);

            // TODO temporary solution to return the current, last and average value
            Random random = new Random();
            results = results.Select(x => new LabelGetFullModel
            {
                Id = x.Id,
                Name = x.Name,
                CurrValue = random.Next(-200, 200),
                LastValue = random.Next(-200, 200),
                AvgValue = random.Next(-200, 200)
            }).ToList();

            return Ok(results);
        }

        // GET api/label/5
        [HttpGet("{id}")]
        [ProducesResponseType((typeof(LabelManageModel)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(long id)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var result = await _labelService.GetByIdAsync(userId, id);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (ForbidException)
            {
                return Forbid();
            }
        }

        // POST api/label/5
        [HttpPost]
        [ProducesResponseType((typeof(LabelManageModel)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post(long group, [FromBody] LabelAddModel value)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var model = await _labelService.AddAsync(userId, group, value);
                if (model == null)
                {
                    return BadRequest();
                }

                return Ok(model);
            }
            catch (ForbidException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return BadRequest();
            }
        }

        // PUT api/label
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] LabelManageModel value)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var model = await _labelService.UpdateAsync(userId, value);
                if (model == null)
                {
                    return BadRequest();
                }

                return Ok(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ForbidException)
            {
                return Forbid();
            }
        }

        // DELETE api/label/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            try
            {
                var result = await _labelService.GetByIdAsync(userId, id);
                if (result == null)
                {
                    return NotFound();
                }
            }
            catch (ForbidException)
            {
                return Forbid();
            }

            if (!await _labelService.DeleteAsync(id))
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
