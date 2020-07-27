using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.Repositories;

namespace MyExpenses.Services
{
    public interface IExpenseService
    {
        Task<ICollection<ExpenseManageModel>> GetAllAsync(string user, long group);
        Task<ICollection<ExpenseFullModel>> GetAllFullAsync(string user, long group, int month, int year);
        Task<ExpenseManageModel> GetByIdAsync(string user, long id);
        Task<ExpenseManageModel> AddAsync(string user, ExpenseAddModel model);
        Task<ICollection<ExpenseManageModel>> AddAsync(string user, long group, ICollection<ExpenseAddModel> models);
        Task<ExpenseManageModel> UpdateAsync(string user, ExpenseManageModel model);
        Task<bool> DeleteAsync(string user, long id);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _repository;
        private readonly IGroupService _groupService;
        private readonly ILabelService _labelService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseService(
                IExpenseRepository repository,
                IGroupService groupService,
                ILabelService labelService,
                IUnitOfWork unitOfWork,
                IMapper mapper)
        {
            _repository = repository;
            _groupService = groupService;
            _labelService = labelService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<ExpenseManageModel>> GetAllAsync(string user, long group)
        {
            var models = await _repository.GetAllAsync();
            var results = models
                .Where(g => g.GroupId.Equals(group) && g.Group.GroupUser.Any(gu => gu.UserId.Equals(user)))
                .OrderBy(x => x.Date).ThenBy(x => x.Name);

            return _mapper.Map<ICollection<ExpenseManageModel>>(results);
        }

        public async Task<ICollection<ExpenseFullModel>> GetAllFullAsync(string user, long group, int month, int year)
        {
            var firstDay = new DateTime(year, month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            var models = await _repository.GetAllAsync();
            var results = models
                .Where(x =>
                    x.GroupId.Equals(group) && x.Group.GroupUser.Any(gu => gu.UserId.Equals(user) &&
                    DateTime.Compare(x.Date, firstDay) > 0 && DateTime.Compare(x.Date, lastDay) < 0))
                .OrderBy(x => x.Date).ThenBy(x => x.Name);

            return _mapper.Map<ICollection<ExpenseFullModel>>(results);
        }

        public async Task<ExpenseManageModel> GetByIdAsync(string user, long id)
        {
            var model = await _repository.GetByIdAsync(id, true);
            if (model != null &&
                !model.Group.GroupUser.Any(gu => gu.UserId.Equals(user)))
            {
                throw new ForbidException();
            }
            return _mapper.Map<ExpenseManageModel>(model);
        }

        public async Task<ExpenseManageModel> AddAsync(string user, ExpenseAddModel model)
        {
            if (model == null)
            {
                return null;
            }

            // label already test the group is valid
            var labelModel = await _labelService.GetByIdAsync(user, model.LabelId);
            if (labelModel == null)
            {
                throw new KeyNotFoundException();
            }

            var objToAdd = _mapper.Map<ExpenseModel>(model);

            _unitOfWork.BeginTransaction();
            var objAdded = await _repository.AddAsync(objToAdd, user);
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? _mapper.Map<ExpenseManageModel>(objAdded) : null;
        }

        public async Task<ICollection<ExpenseManageModel>> AddAsync(string user, long group, ICollection<ExpenseAddModel> models)
        {
            var groupModel = await _groupService.GetByIdAsync(group, user);
            if (groupModel == null)
            {
                throw new KeyNotFoundException(group.ToString());
            }
            if (!groupModel.Users.Any(u => u.Id.Equals(user)))
            {
                throw new ForbidException();
            }

            _unitOfWork.BeginTransaction();
            var resultModels = models.Select(async model =>
            {
                var objToAdd = _mapper.Map<ExpenseModel>(model);
                objToAdd.GroupId = group;

                var objAdded = await _repository.AddAsync(objToAdd, user);
                return _mapper.Map<ExpenseManageModel>(objAdded);
            }).ToArray();
            var returnResults = await Task.WhenAll(resultModels);
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? returnResults : null;
        }

        public async Task<ExpenseManageModel> UpdateAsync(string user, ExpenseManageModel model)
        {
            if (model == null)
            {
                return null;
            }

            // label already test the group is valid
            var labelModel = await _labelService.GetByIdAsync(user, model.LabelId);
            if (labelModel == null)
            {
                throw new KeyNotFoundException();
            }

            var objToUpdate = _mapper.Map<ExpenseModel>(model);

            _unitOfWork.BeginTransaction();
            var updatedModel = await _repository.UpdateAsync(objToUpdate, user);
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? _mapper.Map<ExpenseManageModel>(updatedModel) : null;
        }

        public async Task<bool> DeleteAsync(string user, long id)
        {
            _unitOfWork.BeginTransaction();
            var deleted = await _repository.DeleteAsync(id, user);
            if (!deleted)
            {
                return false;
            }

            var result = await _unitOfWork.CommitAsync();
            return result > 0;
        }
    }
}
