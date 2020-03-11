using AutoMapper;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    public interface IExpenseRepository : IRepository<ExpenseModel, long>
    {
    }

    public class ExpenseRepository : RepositoryBase<ExpenseModel, long>, IExpenseRepository
    {
        public ExpenseRepository(MyExpensesContext context, IMapper mapper) :
            base(context, mapper)
        {
        }
    }
}
