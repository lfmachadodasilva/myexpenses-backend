using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public interface IGroupRepository : IRepository<GroupModel>
    {
    }

    public class GroupRepository : RepositoryBase<GroupModel>, IGroupRepository
    {
        public GroupRepository(
            MyExpensesContext context,
            ILogger<GroupRepository> logger,
            IMapper mapper)
            : base(context, logger, mapper) {}
    }
}
