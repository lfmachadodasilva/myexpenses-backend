using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using MyExpenses.Models;
using MyExpenses.Repositories;
using MyExpenses.Helpers;
using System;
using Microsoft.EntityFrameworkCore;

namespace MyExpenses.Services
{
    public interface ILabelService
    {
        Task<ICollection<LabelManageModel>> GetAllAsync(string user, long group);
        ICollection<LabelGetFullModel> GetAllFull(string user, long group, int month, int year);
        Task<LabelManageModel> GetByIdAsync(string user, long id);
        Task<LabelManageModel> AddAsync(string user, long group, LabelAddModel model);
        Task<ICollection<LabelManageModel>> AddAsync(string user, long group, ICollection<LabelAddModel> models);
        Task<LabelManageModel> UpdateAsync(string user, LabelManageModel model);
        Task<bool> DeleteAsync(long id);
    }

    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _repository;
        private readonly IGroupService _groupService;
        // private readonly IExpenseService _expenseService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LabelService(
            ILabelRepository repository,
            IGroupService groupService,
            // IExpenseService expenseService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _groupService = groupService;
            // _expenseService = expenseService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ICollection<LabelManageModel>> GetAllAsync(string user, long group)
        {
            var models = await _repository.GetAllAsync(x => x.Group);
            var results = models
                .Where(g => g.GroupId.Equals(group) && g.Group.GroupUser.Any(gu => gu.UserId.Equals(user)))
                .OrderBy(x => x.Name);

            return _mapper.Map<ICollection<LabelManageModel>>(results);
        }

        public ICollection<LabelGetFullModel> GetAllFull(string user, long group, int month, int year)
        {
            var currMonthStart = new DateTime(year, month, 1);
            var currMonthEnd = currMonthStart.AddMonths(1).AddDays(-1);

            var lastMonthStart = currMonthStart.AddMonths(-1);
            var lastMonthEnd = currMonthStart.AddDays(-1);

            var averageStart = currMonthStart.AddYears(-100);
            var averageEnd = currMonthStart.AddDays(-1);

            var models = _repository.GetAll()
                .Include(l => l.Group)
                    .ThenInclude(l => l.GroupUser)
                .Include(l => l.Expenses)
                .Where(l => l.GroupId.Equals(group) && l.Group.GroupUser.Any(gu => gu.UserId.Equals(user)))
                .Select(l => new LabelGetFullModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    CurrValue = l.Expenses
                        .Where(e =>
                            DateTime.Compare(e.Date, currMonthStart) > 0 &&
                            DateTime.Compare(e.Date, currMonthEnd) < 0)
                        .Sum(e => e.Value),
                    LastValue = l.Expenses
                        .Where(e =>
                            DateTime.Compare(e.Date, lastMonthStart) > 0 &&
                            DateTime.Compare(e.Date, lastMonthEnd) < 0)
                        .Sum(e => e.Value),
                    AvgValue = l.Expenses
                        .Where(e =>
                            DateTime.Compare(e.Date, averageStart) > 0 &&
                            DateTime.Compare(e.Date, averageEnd) < 0).Any() ?
                                l.Expenses
                                    .Where(e =>
                                        DateTime.Compare(e.Date, averageStart) > 0 &&
                                        DateTime.Compare(e.Date, averageEnd) < 0)
                                    .GroupBy(
                                        key => new { key.Date.Month, key.Date.Year },
                                        value => value.Value,
                                        (key, values) => values.Sum())
                                    .Average() :
                                0
                });
            return models.ToList();
        }

        public async Task<LabelManageModel> GetByIdAsync(string user, long id)
        {
            var model = await _repository.GetByIdAsync(id, true);
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

        public async Task<ICollection<LabelManageModel>> AddAsync(string user, long group, ICollection<LabelAddModel> models)
        {
            var groupModel = await _groupService.GetByIdAsync(group);
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
                var objToAdd = _mapper.Map<LabelModel>(model);
                objToAdd.GroupId = group;

                var objAdded = await _repository.AddAsync(objToAdd);
                return _mapper.Map<LabelManageModel>(objAdded);
            }).ToArray();
            var returnResults = await Task.WhenAll(resultModels);
            var result = await _unitOfWork.CommitAsync();
            return result > 0 ? returnResults : null;
        }

        public async Task<LabelManageModel> UpdateAsync(string user, LabelManageModel model)
        {
            if (model == null)
            {
                return null;
            }

            var labelModel = await _repository.GetByIdAsync(model.Id, true);
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
