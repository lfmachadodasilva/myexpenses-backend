using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Models.Requests;
using lfmachadodasilva.MyExpenses.Api.Properties;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IWebSettings _webSettings;
        private readonly FakeDatabase _fakeDatabase;

        public ExpensesController(
            IExpenseService expenseService,
            IWebSettings webSettings,
            FakeDatabase fakeDatabase)
        {
            _expenseService = expenseService;
            _webSettings = webSettings;
            _fakeDatabase = fakeDatabase;
        }

        // GET api/values
        [HttpGet("years")]
        [ProducesResponseType(typeof(int[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears([FromQuery]long groupId)
        {
            if (_webSettings.UseFakeDatabase)
            {
                var task = Task.Run(() =>
                {
                    var today = DateTime.Today;
                    var years = new List<int>();

                    for (var i = 0; i < 5; i++)
                    {
                        years.Add(today.Year - i);
                    }

                    return years.ToArray();
                });

                return Ok(await task);
            }

            return Ok(await _expenseService.GetAvailableYears(groupId));
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExpenseWithValuesDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery]SearchRequest request)
        {
            if (_webSettings.UseFakeDatabase)
            {
                var task = Task.Run(() =>
                {
                    return _fakeDatabase.Expenses
                        .Where(x =>
                            x.GroupId.Equals(request.GroupId) &&
                            x.Date.Month.Equals(request.Month) &&
                            x.Date.Year.Equals(request.Year))
                        .OrderBy(x => x.Date);
                });
                return Ok(await task);
            }

            var expenses = await _expenseService.GetAllWithValues(request.GroupId, request.Month, request.Year);
            return Ok(expenses);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            if (_webSettings.UseFakeDatabase)
            {
                var task = Task.Run(() =>
                {
                    return _fakeDatabase.Expenses.FirstOrDefault(x => x.Id.Equals(id));

                });
                return Ok(await task);
            }

            var expense = await _expenseService.GetByIdAsync(id);
            return Ok(expense);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] ExpenseDto value)
        {
            if (_webSettings.UseFakeDatabase)
            {
                if (value.Name == "duplicate")
                {
                    return Conflict(Resource.ErrorDuplicate);
                }
                var task = Task.Run(() =>
                {
                    value.Id = _fakeDatabase.Expenses.Count;

                    Random rnd = new Random();
                    var withValues = new ExpenseWithValuesDto
                    {
                        Id = _fakeDatabase.Labels.Count,
                        Name = value.Name,
                        GroupId = value.GroupId,
                        Date = value.Date,
                        Value = value.Value,
                        LabelId = value.LabelId,
                        Type = value.Type
                    };

                    _fakeDatabase.Expenses.Add(withValues);

                    return withValues;
                });

                return Ok(await task);
            }

            var expense = await _expenseService.AddAsync(value);
            return Ok(expense);
        }

        // PUT api/values/5
        [HttpPut]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] ExpenseDto value)
        {
            if (_webSettings.UseFakeDatabase)
            {
                if (value.Name == "duplicate")
                {
                    return Conflict(Resource.ErrorDuplicate);
                }

                var task = Task.Run(() =>
                {
                    var index = _fakeDatabase.Expenses.ToList().FindIndex(x => x.Id.Equals(value.Id));
                    _fakeDatabase.Expenses.ElementAt(index).Name = value.Name;
                    _fakeDatabase.Expenses.ElementAt(index).Date = value.Date;
                    _fakeDatabase.Expenses.ElementAt(index).Value = value.Value;
                    _fakeDatabase.Expenses.ElementAt(index).LabelId = value.LabelId;

                    return _fakeDatabase.Expenses.ElementAt(index);
                });

                return Ok(await task);
            }

            var expense = await _expenseService.UpdateAsync(value);
            return Ok(expense);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task Delete(int id)
        {
            if (_webSettings.UseFakeDatabase)
            {
                var task = Task.Run(() =>
                {
                    var obj = _fakeDatabase.Expenses.FirstOrDefault(x => x.Id.Equals(id));
                    _fakeDatabase.Expenses.Remove(obj);
                });
                await task;
            }

            await _expenseService.RemoveAsync(id);
        }
    }
}
