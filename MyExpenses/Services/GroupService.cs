using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.Repositories;

namespace MyExpenses.Services
{
    public interface IGroupService
    {
        Task<ICollection<GroupGetModel>> GetAllAsync(string user);
        Task<ICollection<GroupGetFullModel>> GetAllFullAsync(string user);
        Task<GroupGetFullModel> GetByIdAsync(long id, string user);
        Task<GroupManageModel> AddAsync(GroupAddModel model, string user);
        Task<GroupManageModel> UpdateAsync(GroupManageModel model, string user);
        Task<bool> DeleteAsync(long id, string user);
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<GroupGetModel>> GetAllAsync(string user)
        {
            var models = await _repository.GetAllAsync();
            var results = models
                .Where(g => g.GroupUser.Any(gu => gu.UserId.Equals(user)))
                .OrderBy(g => g.Name);

            return _mapper.Map<ICollection<GroupGetModel>>(results);
        }

        public async Task<ICollection<GroupGetFullModel>> GetAllFullAsync(string user)
        {
            var models = await _repository.GetAllAsync();
            var results = models
                .Where(g => g.GroupUser.Any(gu => gu.UserId.Equals(user)))
                .OrderBy(g => g.Name);

            return _mapper.Map<ICollection<GroupGetFullModel>>(results);
        }

        public async Task<GroupGetFullModel> GetByIdAsync(long id, string user)
        {
            var model = await _repository.GetByIdAsync(id);
            return _mapper.Map<GroupGetFullModel>(model);
        }

        public async Task<GroupManageModel> AddAsync(GroupAddModel model, string user)
        {
            _unitOfWork.BeginTransaction();
            var addedModel = await _repository.AddAsync(_mapper.Map<GroupModel>(model), user);
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? _mapper.Map<GroupManageModel>(addedModel) : null;
        }

        public async Task<GroupManageModel> UpdateAsync(GroupManageModel model, string user)
        {
            _unitOfWork.BeginTransaction();
            var updatedModel = await _repository.UpdateAsync(_mapper.Map<GroupModel>(model), user);
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? _mapper.Map<GroupManageModel>(updatedModel) : null;
        }

        public async Task<bool> DeleteAsync(long id, string user)
        {
            _unitOfWork.BeginTransaction();
            var deleted = await _repository.DeleteAsync(id, user);
            if (!deleted)
            {
                return false;
            }

            var result = await _unitOfWork.CommitAsync();
            return result > 0;

            // TODO delete labels and expenses
        }
    }
}
