using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    public interface IGroupRepository : IRepository<GroupModel, long>
    {
        Task<bool> Validate(long groupId, string userId);
    }

    public class GroupRepository : RepositoryBase<GroupModel, long>, IGroupRepository
    {
        private readonly MyExpensesContext _context;

        public GroupRepository(MyExpensesContext context, IMapper mapper) :
            base(context, mapper)
        {
            _context = context;
        }

        public Task<bool> Validate(long groupId, string userId)
        {
            return _context.GroupUser.AnyAsync(x => x.GroupId.Equals(groupId) && x.UserId.Equals(userId));
        }
    }
}
