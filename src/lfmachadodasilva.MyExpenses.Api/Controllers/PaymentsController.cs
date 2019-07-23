using System;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        public PaymentsController(FakeDatabase db)
        {
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(PaymentWithValueDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery]SearchRequest request)
        {
            var task = Task.Run(() =>
            {
                return FakeDatabase.Payments.Where(x => x.GroupId.Equals(request.GroupId));
            });
            return Ok(await task);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var task = Task.Run(() =>
            {
                return FakeDatabase.Payments.FirstOrDefault(x => x.Id.Equals(id));
            });
            return Ok(await task);
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Add([FromBody] PaymentDto value)
        {
            var task = Task.Run(() =>
            {
                Random rnd = new Random();
                var withValues = new PaymentWithValueDto
                {
                    Id = FakeDatabase.Payments.Count(),
                    Name = value.Name,
                    GroupId = value.GroupId,
                    CurrentValue = rnd.Next(1, 250),
                    AverageValue = rnd.Next(1, 250),
                    LastValue = rnd.Next(1, 250)
                };

                FakeDatabase.Payments.Add(withValues);
                return withValues;
            });
            
            return Ok(await task);
        }

        // PUT api/values/5
        [HttpPut()]
        [ProducesResponseType(typeof(PaymentDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit([FromBody] PaymentDto value)
        {
            var task = Task.Run(() =>
            {
                var label = FakeDatabase.Payments.FirstOrDefault(x => x.Id.Equals(value.Id));
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
                var label = FakeDatabase.Payments.FirstOrDefault(x => x.Id.Equals(id));
                FakeDatabase.Payments.Remove(label);
            });
            await task;
        }
    }
}
