using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly FakeDatabase _fakeDatabase;

        public UsersController(IUserService userService, FakeDatabase fakeDatabase)
        {
            _userService = userService;
            _fakeDatabase = fakeDatabase;
        }

        // GET api/values
        [HttpGet]
        [ProducesResponseType(typeof(UserDto[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var task = Task.Run(() => _fakeDatabase.Users);
            return Ok(await task);
        }
    }
}
