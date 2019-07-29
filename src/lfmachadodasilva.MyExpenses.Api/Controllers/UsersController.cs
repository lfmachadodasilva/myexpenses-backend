using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController(FakeDatabase db)
        {        
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(UserDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var task = Task.Run(() => FakeDatabase.Users);
            return Ok(await task);
        }
    }
}
