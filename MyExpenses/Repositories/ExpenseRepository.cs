using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    public interface IExpenseRepository : IRepository<ExpenseModel, long>
    {
    }

    public class ExpenseRepository : RepositoryBase<ExpenseModel, long>, IExpenseRepository
    {
        private readonly MyExpensesContext _context;

        public ExpenseRepository(MyExpensesContext context, IMapper mapper) :
            base(context, mapper)
        {
            _context = context;
        }

        public override Task<List<ExpenseModel>> GetAllAsync(params Expression<Func<ExpenseModel, object>>[] includes)
        {
            //_logger.LogInformation("get all");
            return _context.Expenses
                .Include(l => l.Group)
                    .ThenInclude(g => g.GroupUser)
                .Include(l => l.Label)
                .ToListAsync();
        }

        public override Task<ExpenseModel> GetByIdAsync(long id, bool include)
        {
            if (include)
            {
                //_logger.LogInformation("get all");
                return _context.Expenses
                    .Include(l => l.Group)
                        .ThenInclude(g => g.GroupUser)
                            .ThenInclude(gu => gu.User)
                    .Include(l => l.Label)
                    .FirstOrDefaultAsync(x => x.Id.Equals(id));
            }

            return base.GetByIdAsync(id);
        }
    }
}
