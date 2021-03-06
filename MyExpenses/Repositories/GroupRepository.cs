using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    public interface IGroupRepository : IRepository<GroupModel, long>
    {
    }

    public class GroupRepository : RepositoryBase<GroupModel, long>, IGroupRepository
    {
        private readonly MyExpensesContext _context;

        public GroupRepository(MyExpensesContext context, IMapper mapper) :
            base(context, mapper)
        {
            _context = context;
        }

        public override Task<List<GroupModel>> GetAllAsync(params Expression<Func<GroupModel, object>>[] includes)
        {
            //_logger.LogInformation("get all");
            return _context.Groups
                .Include(g => g.GroupUser)
                    .ThenInclude(gu => gu.User)
                .ToListAsync();
        }

        public override Task<GroupModel> GetByIdAsync(long id, bool include)
        {
            //_logger.LogInformation("get all");
            return _context.Groups
                .Include(g => g.GroupUser)
                    .ThenInclude(gu => gu.User)
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
    }
}
