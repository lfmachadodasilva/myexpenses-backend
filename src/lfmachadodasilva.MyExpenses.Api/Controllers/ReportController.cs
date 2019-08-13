using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
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
        private readonly FakeDatabase _fakeDatabase;

        public ReportController(IReportService reportService, FakeDatabase fakeDatabase)
        {
            _reportService = reportService;
            _fakeDatabase = fakeDatabase;
        }

        // GET
        [HttpGet]
        [ProducesResponseType(typeof(ReportDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReportByYear(ReportRequest request)
        {
            var task = Task.Run(() =>
            {
                var report = new ReportDto();

                var expenses = _fakeDatabase.Expenses
                    .Where(e => e.Date.Year.Equals(request.Year) && e.GroupId.Equals(request.GroupId));

                report.ByLabel = expenses
                    .GroupBy(x => x.LabelId)
                    .Select(x => SelectByLabel(x.Key, x.ToList()));

                report.ByMonth = expenses
                    .GroupBy(x => x.Date.Month)
                    .Select(x => SelectByMonth(x.Key, x.ToList()));

                var outcoming = expenses.Where(x => x.Type.Equals(ExpenseType.Outcoming));
                var incoming = expenses.Where(x => x.Type.Equals(ExpenseType.Incoming));

                var outcomingSum = outcoming.Sum(x => x.Value);
                var incomingSum = incoming.Sum(x => x.Value);

                var outcomingAverage = outcoming.Sum(x => x.Value);
                var incomingAverage = incoming.Sum(x => x.Value);

                report.ByYear = new ReportByYear
                {
                    Year = request.Year,
                    Total = outcomingSum,
                    TotalAverage = outcomingAverage,
                    Left = outcomingSum - incomingSum,
                    LeftAverage = outcomingAverage - incomingAverage
                };

                return report;
            });
            return Ok(await task);
        }

        private ReportByLabelDto SelectByLabel(long labelId, IEnumerable<ExpenseDto> expenses)
        {
            return new ReportByLabelDto
            {
                LabelName = labelId.ToString(),
                AverageValue = expenses.Average(x => x.Value)
            };
        }

        private ReportByMonth SelectByMonth(int month, IEnumerable<ExpenseDto> expenses)
        {
            var outcomingSum = expenses.Where(x => x.Type.Equals(ExpenseType.Outcoming)).Sum(x => x.Value);
            var incomingSum = expenses.Where(x => x.Type.Equals(ExpenseType.Incoming)).Sum(x => x.Value);

            return new ReportByMonth
            {
                Month = month,
                Total = outcomingSum,
                Left = outcomingSum - incomingSum
            };
        }
    }
}
