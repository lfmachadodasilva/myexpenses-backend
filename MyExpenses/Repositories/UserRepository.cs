using AutoMapper;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    public interface IUserRepository : IRepository<UserModel, string>
    {
    }

    public class UserRepository : RepositoryBase<UserModel, string>, IUserRepository
    {
        public UserRepository(MyExpensesContext context, IMapper mapper) :
            base(context, mapper)
        {
        }
    }
}
