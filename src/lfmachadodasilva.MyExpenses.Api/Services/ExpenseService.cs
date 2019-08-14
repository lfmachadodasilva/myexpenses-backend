using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Repositories;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public interface IExpenseService : IService<ExpenseModel, ExpenseDto>
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="groupId">group id</param>
        /// <param name="month">month</param>
        /// <param name="year">year</param>
        /// <returns>mmodels</returns>
        Task<IEnumerable<ExpenseWithValuesDto>> GetAllWithValues(long groupId, int month, int year);

        Task<IEnumerable<int>> GetAvailableYears(long groupId);
    }

    public class ExpenseService : ServiceBase<ExpenseModel, ExpenseDto>, IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMapper _mapper;

        public ExpenseService(
            IExpenseRepository expenseRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(expenseRepository, unitOfWork, mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<ExpenseWithValuesDto>> GetAllWithValues(long groupId, int month, int year)
        {
            // get all expenses
            var expensesTask = _expenseRepository.GetAllAsyncEnumerable(x => x.Label);

            // create query
            var currentExpensesTask = expensesTask.Where(x =>
                x.GroupId.Equals(groupId) && x.Date.Month.Equals(month) && x.Date.Year.Equals(year));

            // execute database query
            var expenses = await currentExpensesTask.ToList();

            // map to DTO
            return _mapper.Map<IEnumerable<ExpenseWithValuesDto>>(expenses);
        }

        public async Task<IEnumerable<int>> GetAvailableYears(long groupId)
        {
            // get all expenses
            var expensesTask = _expenseRepository.GetAllAsyncEnumerable();

            // create query
            var yearsTask = expensesTask
                .Where(x => x.GroupId.Equals(groupId))
                .Select(x => x.Date.Year)
                .Distinct();

            // execute query
            return await yearsTask.ToList();
        }
    }
}
