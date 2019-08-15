using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Models.Requests;
using lfmachadodasilva.MyExpenses.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET
        [HttpGet]
        [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportByYear(ReportRequest request)
        {
            var report = await _reportService.GetReport(request.GroupId, request.Year);
            return Ok(report);
        }
    }
}
