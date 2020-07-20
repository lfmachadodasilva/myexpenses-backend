using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyExpenses.Helpers;

namespace MyExpenses.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DbController : ControllerBase
    {
        private readonly MyExpensesContext _context;
        private readonly IValidateHelper _validateHelper;

        public DbController(MyExpensesContext context, IValidateHelper validateHelper)
        {
            _context = context;
            _validateHelper = validateHelper;
        }

        [HttpGet("migrate")]
        public async Task<IActionResult> Migrate()
        {
            await _context.Database.MigrateAsync();
            return Ok();
        }

        [HttpGet("seed")]
        public IActionResult Seed()
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            if (userId != "13FAoQ4yNNSl7mUJtQgTQpFeWmU2")
            {
                return Forbid();
            }

            new MyExpensesSeed(_context).Run();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var userId = _validateHelper.GetUserId(HttpContext);
            if (userId != "13FAoQ4yNNSl7mUJtQgTQpFeWmU2")
            {
                return Forbid();
            }
            await _context.Database.EnsureDeletedAsync();
            return Ok();
        }
    }
}
