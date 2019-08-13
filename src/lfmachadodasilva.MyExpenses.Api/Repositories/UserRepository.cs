using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public interface IUserRepository : IRepository<UserModel>
    {
    }

    public class UserRepository : RepositoryBase<UserModel>, IUserRepository
    {
        public UserRepository(
            MyExpensesContext context,
            ILogger<UserRepository> logger,
            IMapper mapper)
            : base(context, logger, mapper)
        { }
    }
}
