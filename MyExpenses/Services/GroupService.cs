using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyExpenses.Models;
using MyExpenses.Repositories;

namespace MyExpenses.Services
{
    public interface IGroupService
    {
        Task<ICollection<GroupGetModel>> GetAllAsync(string userId);
        Task<ICollection<GroupGetFullModel>> GetAllFullAsync(string userId);
        Task<GroupGetFullModel> GetByIdAsync(long id);
        Task<GroupManageModel> AddAsync(GroupAddModel model);
        Task<GroupManageModel> UpdateAsync(GroupManageModel model);
        Task<bool> DeleteAsync(long id);
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

        public async Task<ICollection<GroupGetModel>> GetAllAsync(string userId)
        {
            var models = await _repository.GetAllAsync();
            var modelsFiltered = models.Where(g => g.GroupUser.Any(gu => gu.UserId.Equals(userId)));

            return _mapper.Map<ICollection<GroupGetModel>>(modelsFiltered);
        }

        public async Task<ICollection<GroupGetFullModel>> GetAllFullAsync(string userId)
        {
            var models = await _repository.GetAllAsync();
            var modelsFiltered = models.Where(g => g.GroupUser.Any(gu => gu.UserId.Equals(userId)));

            return _mapper.Map<ICollection<GroupGetFullModel>>(modelsFiltered);
        }

        public async Task<GroupGetFullModel> GetByIdAsync(long id)
        {
            var model = await _repository.GetByIdAsync(id);
            return _mapper.Map<GroupGetFullModel>(model);
        }

        public async Task<GroupManageModel> AddAsync(GroupAddModel model)
        {
            _unitOfWork.BeginTransaction();
            var addedModel = await _repository.AddAsync(_mapper.Map<GroupModel>(model));
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? _mapper.Map<GroupManageModel>(addedModel) : null;
        }

        public async Task<GroupManageModel> UpdateAsync(GroupManageModel model)
        {
            _unitOfWork.BeginTransaction();
            var updatedModel = await _repository.UpdateAsync(_mapper.Map<GroupModel>(model));
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? _mapper.Map<GroupManageModel>(updatedModel) : null;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            _unitOfWork.BeginTransaction();
            var deleted = await _repository.DeleteAsync(id);
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
