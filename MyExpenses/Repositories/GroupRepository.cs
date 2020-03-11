using AutoMapper;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    public interface IGroupRepository : IRepository<GroupModel, long>
    {
    }

    public class GroupRepository : RepositoryBase<GroupModel, long>, IGroupRepository
    {
        public GroupRepository(MyExpensesContext context, IMapper mapper) :
            base(context, mapper)
        {
        }
    }
}
