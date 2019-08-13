using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Repositories;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public interface IUserService : IService<UserModel, UserDto>
    {
    }

    public class UserService : ServiceBase<UserModel, UserDto>, IUserService
    {
        public UserService(
            IUserRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(repository, unitOfWork, mapper) {}
    }
}
