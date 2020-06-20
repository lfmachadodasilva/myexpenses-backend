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
        // Task<UserModel> GetAsync(string id);
        // Task<UserModel> UpdateAsync(UserModel value);
        // Task<UserModel> AddAsync(UserModel value);
        Task<UserModel> AddOrUpdateAsync(UserModel value);
        // Task DeleteAsync(string id);
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
            return await _repository.GetAll();
        }

        // public async Task<UserModel> GetAsync(string id)
        // {
        //     return await _repository.GetByIdAsync(id);
        // }

        // public async Task<UserModel> UpdateAsync(UserModel value)
        // {
        //     _unitOfWork.BeginTransaction();
        //     var modelUpdated = await _repository.UpdateAsync(value);
        //     await _unitOfWork.CommitAsync();
        //     return modelUpdated;
        // }

        // public async Task<UserModel> AddAsync(UserModel value)
        // {
        //     _unitOfWork.BeginTransaction();
        //     // var model = _mapper.Map<UserModel>(value);
        //     var modelAdded = await _repository.AddAsync(value);
        //     await _unitOfWork.CommitAsync();
        //     return modelAdded;
        // }

        public async Task<UserModel> AddOrUpdateAsync(UserModel value)
        {
            _unitOfWork.BeginTransaction();
            // var model = _mapper.Map<UserModel>(value);
            var modelAdded = await _repository.AddOrUpdateAsync(value);
            await _unitOfWork.CommitAsync();
            return modelAdded;
        }

        // public async Task DeleteAsync(string id)
        // {
        //     _unitOfWork.BeginTransaction();
        //     var deleted = await _repository.DeleteAsync(id);
        //     await _unitOfWork.CommitAsync();
        // }
    }
}
