using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly ILabelService _labelService;
        private readonly IValidateHelper _validateHelper;

        public ExpenseController(
            IExpenseService expenseService,
            ILabelService labelService,
            IValidateHelper validateHelper)
        {
            _expenseService = expenseService;
            _labelService = labelService;
            _validateHelper = validateHelper;
        }

        // GET api/expense
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<ExpenseManageModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(long group)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var results = await _expenseService.GetAllAsync(userId, group);
            return Ok(results);
        }

        // GET api/expense/full
        [HttpGet("full")]
        [ProducesResponseType(typeof(ICollection<ExpenseFullModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull(long group, int month, int year)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var results = await _expenseService.GetAllFullAsync(userId, group, month, year);
            return Ok(results);
        }

        // GET api/expense/5
        [HttpGet("{id}")]
        [ProducesResponseType((typeof(ExpenseManageModel)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(long id)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var result = await _expenseService.GetByIdAsync(userId, id);
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

        // POST api/expense/5
        [HttpPost]
        [ProducesResponseType((typeof(ExpenseManageModel)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] ExpenseAddModel value)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var model = await _expenseService.AddAsync(userId, value);
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

        // PUT api/expense
        [HttpPut]
        [ProducesResponseType((typeof(ExpenseManageModel)), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] ExpenseManageModel value)
        {
            var userId = _validateHelper.GetUserId(HttpContext);

            try
            {
                var model = await _expenseService.UpdateAsync(userId, value);
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

        // DELETE api/expense/5
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

        // POST api/expense/import
        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Import(long group, [FromBody] DataModel data)
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            var labels = await _labelService.GetAllAsync(userId, group);
            var errors = new List<string>();

            var rows = data.Data.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var models = rows.Select(row =>
                {
                    var fields = row.Split(",");

                    DateTime date;
                    if (!DateTime.TryParseExact(fields[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        errors.Add($"Date {fields[0]} does not match to dd/mm/yyyy format");
                    }

                    var name = fields[1];
                    if (string.IsNullOrEmpty(name))
                    {
                        errors.Add($"Label {fields[1]} can not be null or empty");
                    }

                    var label = labels.FirstOrDefault(l => l.Name.Equals(fields[2]));
                    if (label == null)
                    {
                        errors.Add($"Label {fields[2]} does not exists");
                    }

                    decimal value;
                    if (!decimal.TryParse(fields[3], out value))
                    {
                        errors.Add($"Value {fields[3]} does not match as a number");
                    }

                    return new ExpenseAddModel
                    {
                        // 0
                        Date = date,
                        // 1
                        Name = name,
                        // 2
                        LabelId = label?.Id ?? 0,
                        // 3
                        Value = value,

                        GroupId = group
                    };
                }
            ).ToList();

            if (errors.Any())
            {
                return BadRequest(errors);
            }

            var results = await _expenseService.AddAsync(userId, group, models);
            return Ok(results);
        }
    }
}
