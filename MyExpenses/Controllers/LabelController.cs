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
    public class DataModel
    {
        public string Data { get; set; }
    }

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType((typeof(LabelManageModel)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                if (!await _labelService.DeleteAsync(userId, id))
                {
                    return BadRequest();
                }
            }
            catch (ForbidException)
            {
                return Forbid();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }

        // POST api/label/import
        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Import(long group, [FromBody] DataModel data)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var errors = new List<string>();
            var rows = data.Data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var models = rows.Select(row =>
                {
                    var fields = row.Split(",");

                    var name = fields[0];
                    if (string.IsNullOrEmpty(name))
                    {
                        errors.Add($"Label {fields[0]} can not be null or empty");
                    }

                    return new LabelAddModel { Name = name };
                }
            ).ToList();

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            var results = await _labelService.AddAsync(userId, group, models);
            return Ok(results);
        }
    }
}
