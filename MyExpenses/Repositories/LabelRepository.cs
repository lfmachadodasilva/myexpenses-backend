using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    public interface ILabelRepository : IRepository<LabelModel, long>
    {
        Task<List<LabelModel>> GetAllWithIncludeAsync();
    }

    public class LabelRepository : RepositoryBase<LabelModel, long>, ILabelRepository
    {
        private readonly MyExpensesContext _context;

        public LabelRepository(MyExpensesContext context, IMapper mapper) :
            base(context, mapper)
        {
            _context = context;
        }

        public override Task<List<LabelModel>> GetAllAsync(params Expression<Func<LabelModel, object>>[] includes)
        {
            //_logger.LogInformation("get all");
            return GetAll()
                .Include(l => l.Group)
                    .ThenInclude(g => g.GroupUser)
                .ToListAsync();
        }

        public Task<List<LabelModel>> GetAllWithIncludeAsync()
        {
            //_logger.LogInformation("get all");
            return GetAll()
                .Include(l => l.Group)
                    .ThenInclude(g => g.GroupUser)
                .Include(l => l.Expenses)
                .Include(l => l.CreatedBy)
                .Include(l => l.UpdatedBy)
                .ToListAsync();
        }

        public override Task<LabelModel> GetByIdAsync(long id, bool include)
        {
            if (include)
            {
                //_logger.LogInformation("get all");
                return GetAll()
                    .Include(l => l.Group)
                        .ThenInclude(g => g.GroupUser)
                            .ThenInclude(gu => gu.User)
                    .Include(l => l.CreatedBy)
                    .Include(l => l.UpdatedBy)
                    .FirstOrDefaultAsync(x => x.Id.Equals(id));
            }

            return base.GetByIdAsync(id);
        }
    }
}
