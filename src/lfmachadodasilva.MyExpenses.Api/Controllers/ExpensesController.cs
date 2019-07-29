using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Models.Requests;
using lfmachadodasilva.MyExpenses.Api.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        public ExpensesController(FakeDatabase db)
        {
        }

        // GET api/values
        [HttpGet("years")]
        [ProducesResponseType(typeof(int[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetYears()
        {
            var task = Task.Run(() =>
            {
                var today = DateTime.Today;
                var years = new List<int>();

                for (int i = 0; i < 5; i++)
                {
                    years.Add(today.Year - i);
                }

                return years.ToArray();
            });
            
            return Ok(await task);
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(ExpenseWithValuesDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery]SearchRequest request)
        {
            var task = Task.Run(() =>
            {
                return FakeDatabase.Expenses
                    .Where(x => 
                        x.GroupId.Equals(request.GroupId) &&
                        x.Date.Month.Equals(request.Month) &&
                        x.Date.Year.Equals(request.Year));
            });
            return Ok(await task);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var task = Task.Run(() =>
            {
                return FakeDatabase.Expenses.FirstOrDefault(x => x.Id.Equals(id));

            });
            return Ok(await task);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] ExpenseDto value)
        {
            if (value.Name == "duplicate")
            {
                return Conflict(Resource.ErrorDuplicate);
            }
            var task = Task.Run(() =>
            {
                value.Id = FakeDatabase.Expenses.Count;

                Random rnd = new Random();
                var withValues = new ExpenseWithValuesDto
                {
                    Id = FakeDatabase.Labels.Count,
                    Name = value.Name,
                    GroupId = value.GroupId,
                    Date = value.Date,
                    Value = value.Value,
                    LabelId = value.LabelId,
                    PaymentId = value.PaymentId,
                    AverageValue = rnd.Next(1, 250),
                    LastValue = rnd.Next(1, 250)
                };

                FakeDatabase.Expenses.Add(withValues);

                return withValues;
            });

            return Ok(await task);
        }

        // PUT api/values/5
        [HttpPut]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] ExpenseDto value)
        {
            if (value.Name == "duplicate")
            {
                return Conflict(Resource.ErrorDuplicate);
            }

            var task = Task.Run(() =>
            {
                var obj = FakeDatabase.Expenses.FirstOrDefault(x => x.Id.Equals(value.Id));
                obj.Name = value.Name;
                obj.Date = value.Date;
                obj.Value = value.Value;
                obj.LabelId = value.LabelId;
                obj.PaymentId = value.PaymentId;

                return obj;
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
                var obj = FakeDatabase.Expenses.FirstOrDefault(x => x.Id.Equals(id));
                FakeDatabase.Expenses.Remove(obj);
            });
            await task;
        }
    }
}
