using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public interface IExpenseRepository : IRepository<ExpenseModel>
    {
    }

    public class ExpenseRepository : RepositoryBase<ExpenseModel>, IExpenseRepository
    {
        public ExpenseRepository(
            MyExpensesContext context,
            ILogger<ExpenseRepository> logger,
            IMapper mapper)
            : base(context, logger, mapper) {}
    }
}
