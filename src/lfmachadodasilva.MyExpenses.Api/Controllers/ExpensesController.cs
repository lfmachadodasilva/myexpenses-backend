using System.Collections.Generic;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Models.Requests;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // GET api/values
        [HttpGet("years")]
        [Authorize]
        [ProducesResponseType(typeof(int[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears([FromQuery]long groupId)
        {
            var years = await _expenseService.GetAvailableYears(groupId);
            return Ok(years);
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExpenseWithValuesDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery]SearchRequest request)
        {
            var expenses = await _expenseService.GetAllWithValues(request.GroupId, request.Month, request.Year);
            return Ok(expenses);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var expense = await _expenseService.GetByIdAsync(id);
            return Ok(expense);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] ExpenseDto value)
        {
            var expense = await _expenseService.AddAsync(value);
            return Ok(expense);
        }

        // PUT api/values/5
        [HttpPut]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] ExpenseDto value)
        {
            var expense = await _expenseService.UpdateAsync(value);
            return Ok(expense);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task Delete(int id)
        {
            await _expenseService.RemoveAsync(id);
        }
    }
}
