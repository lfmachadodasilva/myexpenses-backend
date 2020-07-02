using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using MyExpenses.Models;
using MyExpenses.Repositories;
using MyExpenses.Helpers;

namespace MyExpenses.Services
{
    public interface ILabelService
    {
        Task<ICollection<LabelManageModel>> GetAllAsync(string user, long group);
        Task<ICollection<LabelGetFullModel>> GetAllFullAsync(string user, long group, int month, int year);
        Task<LabelManageModel> GetByIdAsync(string user, long id);
        Task<LabelManageModel> AddAsync(string user, long group, LabelAddModel model); Task<LabelManageModel> UpdateAsync(string user, LabelManageModel model);
        Task<bool> DeleteAsync(long id);
    }

    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _repository;
        private readonly IGroupService _groupService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LabelService(ILabelRepository repository, IGroupService groupService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _groupService = groupService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<LabelManageModel>> GetAllAsync(string user, long group)
        {
            var models = await _repository.GetAllAsync(x => x.Group);
            var modelsFiltered = models.Where(g => g.GroupId.Equals(group) && g.Group.GroupUser.Any(gu => gu.UserId.Equals(user)));

            return _mapper.Map<ICollection<LabelManageModel>>(modelsFiltered);
        }

        public async Task<ICollection<LabelGetFullModel>> GetAllFullAsync(string user, long group, int month, int year)
        {
            var models = await _repository.GetAllAsync(x => x.Group);
            var modelsFiltered = models.Where(g => g.GroupId.Equals(group) && g.Group.GroupUser.Any(gu => gu.UserId.Equals(user)));

            // TODO join with labels to return current, last and average value 

            return _mapper.Map<ICollection<LabelGetFullModel>>(modelsFiltered);
        }

        public async Task<LabelManageModel> GetByIdAsync(string user, long id)
        {
            var model = await _repository.GetByIdAsync(id);
            if (model != null && !model.Group.GroupUser.Any(gu => gu.UserId.Equals(user)))
            {
                throw new ForbidException();
            }
            return _mapper.Map<LabelManageModel>(model);
        }

        public async Task<LabelManageModel> AddAsync(string user, long group, LabelAddModel model)
        {
            if (model == null)
            {
                return null;
            }

            var groupModel = await _groupService.GetByIdAsync(group);
            if (groupModel == null)
            {
                throw new KeyNotFoundException(group.ToString());
            }
            if (!groupModel.Users.Any(u => u.Id.Equals(user)))
            {
                throw new ForbidException();
            }

            var objToAdd = _mapper.Map<LabelModel>(model);
            objToAdd.GroupId = group;

            _unitOfWork.BeginTransaction();
            var objAdded = await _repository.AddAsync(objToAdd);
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? _mapper.Map<LabelManageModel>(objAdded) : null;
        }

        public async Task<LabelManageModel> UpdateAsync(string user, LabelManageModel model)
        {
            if (model == null)
            {
                return null;
            }

            var labelModel = await _repository.GetByIdAsync(model.Id);
            if (labelModel == null)
            {
                throw new KeyNotFoundException(model.Id.ToString());
            }
            if (!labelModel.Group.GroupUser.Any(gu => gu.UserId.Equals(user)))
            {
                throw new ForbidException();
            }

            var objToUpdate = _mapper.Map<LabelModel>(model);
            objToUpdate.GroupId = labelModel.GroupId;

            _unitOfWork.BeginTransaction();
            var updatedModel = await _repository.UpdateAsync(objToUpdate);
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? _mapper.Map<LabelManageModel>(updatedModel) : null;
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

            // TODO delete expenses
        }
    }
}
