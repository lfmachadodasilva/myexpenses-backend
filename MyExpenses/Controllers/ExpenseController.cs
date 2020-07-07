using System.Collections.Generic;
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
        private readonly IValidateHelper _validateHelper;

        public ExpenseController(IExpenseService labelService, IValidateHelper validateHelper)
        {
            _expenseService = labelService;
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
                var result = await _expenseService.GetByIdAsync(userId, id);
                if (result == null)
                {
                    return NotFound();
                }
            }
            catch (ForbidException)
            {
                return Forbid();
            }

            if (!await _expenseService.DeleteAsync(id))
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
