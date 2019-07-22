using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        public GroupsController(FakeDatabase db)
        {
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(GroupDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var task = Task.Run(() => { return FakeDatabase.Groups; });
            return Ok(await task);
        }
    }
}
