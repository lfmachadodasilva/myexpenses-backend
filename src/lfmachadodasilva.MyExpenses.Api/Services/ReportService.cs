using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Repositories;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public interface IReportService
    {
        Task<ReportDto> GetReport(long groupId, int year);
    }

    public class ReportService : IReportService
    {
        private readonly IExpenseRepository _expenseRepository;

        public ReportService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public async Task<ReportDto> GetReport(long groupId, int year)
        {
            var expensesTask = _expenseRepository
                .GetAllAsyncEnumerable(e => e.Label)
                .Where(e => e.Date.Year.Equals(year) && e.GroupId.Equals(groupId));

            var expenses = await expensesTask.ToList();

            var byLabelTask = Task.Run(() =>
            {
                return expenses
                    .GroupBy(x => x.LabelId)
                    .Select(x => SelectByLabel(x.Key, x.ToList()));
            });

            var byMonthTask = Task.Run(() =>
            {
                return expenses
                    .GroupBy(x => x.Date.Month)
                    .Select(x => SelectByMonth(x.Key, x.ToList()));
            });

            var byYearTask = Task.Run(() =>
            {
                var outcoming = expenses
                    .Where(x => x.Type.Equals(ExpenseType.Outcoming))
                    .Select(x => x.Value);
                var incoming = expenses
                    .Where(x => x.Type.Equals(ExpenseType.Incoming))
                    .Select(x => x.Value);

                var outcomingSum = outcoming.Sum();
                var incomingSum = incoming.Sum();

                var outcomingAverage = outcoming.DefaultIfEmpty(0).Average();
                var incomingAverage = incoming.DefaultIfEmpty(0).Average();

                return new ReportByYear
                {
                    Year = year,
                    Total = outcomingSum,
                    TotalAverage = outcomingAverage,
                    Left = outcomingSum - incomingSum,
                    LeftAverage = outcomingAverage - incomingAverage
                };
            });

            // run in parallel
            await Task.WhenAll(byLabelTask, byMonthTask, byYearTask);

            return new ReportDto
            {
                ByLabel = await byLabelTask,
                ByMonth = await byMonthTask,
                ByYear = await byYearTask,
            };
        }

        private static ReportByLabelDto SelectByLabel(long labelId, IEnumerable<ExpenseModel> expenses)
        {
            return new ReportByLabelDto
            {
                LabelName = labelId.ToString(),
                AverageValue = expenses.Select(x => x.Value).DefaultIfEmpty(0).Average()
            };
        }

        private static ReportByMonth SelectByMonth(int month, IEnumerable<ExpenseModel> expenses)
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
