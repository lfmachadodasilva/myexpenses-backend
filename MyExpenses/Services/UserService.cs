using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MyExpenses.Models;
using MyExpenses.Repositories;

namespace MyExpenses.Services
{
    public interface IUserService
    {
        Task<ICollection<UserModel>> GetAllAsync();
        Task<UserModel> AddOrUpdateAsync(UserModel value);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ICollection<UserModel>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<UserModel> AddOrUpdateAsync(UserModel value)
        {
            _unitOfWork.BeginTransaction();
            var modelAdded = await _repository.AddOrUpdateAsync(value);
            await _unitOfWork.CommitAsync();
            return modelAdded;
        }
    }
}
