using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
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
    }
}
