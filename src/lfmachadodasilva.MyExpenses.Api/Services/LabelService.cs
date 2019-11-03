﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Repositories;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public interface ILabelService : IService<LabelModel, LabelDto>
    {
        /// <summary>
        /// Get all with values
        /// </summary>
        /// <param name="groupId">group id</param>
        /// <param name="month">month</param>
        /// <param name="year">year</param>
        /// <returns>models</returns>
        Task<IEnumerable<LabelWithValuesDto>> GetAllWithValuesAsync(long groupId, int month, int year);

        Task<LabelDto> AddAsync(LabelAddDto dto);

        Task<IEnumerable<LabelDto>> GetAllAsync(long groupId);
    }

    public class LabelService : ServiceBase<LabelModel, LabelDto>, ILabelService
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LabelService(
            ILabelRepository labelRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(labelRepository, unitOfWork, mapper)
        {
            _labelRepository = labelRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LabelWithValuesDto>> GetAllWithValuesAsync(long groupId, int month, int year)
        {
            var labelsTask = _labelRepository
                .GetAllAsyncEnumerable(x => x.Expenses);

            var labelsWithValueTask = labelsTask
                    .Select(label =>
                    {
                        var labelWithValues = _mapper.Map<LabelWithValuesDto>(label);

                        labelWithValues.CurrentValue = label.Expenses
                            .Where(e =>
                                e.GroupId.Equals(groupId) &&
                                e.Date.Month.Equals(month) &&
                                e.Date.Year.Equals(year))
                            .Select(e => e.Value)
                            .Sum();

                        var currentDate = new DateTime(year, month, 1);
                        var lastMonth = currentDate.AddMonths(-1);

                        labelWithValues.LastValue = label.Expenses
                            .Where(e =>
                                e.GroupId.Equals(groupId) &&
                                e.Date.Month.Equals(lastMonth.Month) &&
                                e.Date.Year.Equals(lastMonth.Year))
                            .Select(e => e.Value)
                            .Sum();

                        labelWithValues.AverageValue = label.Expenses
                            .Where(e =>
                                e.GroupId.Equals(groupId) &&
                                e.Date < currentDate)
                            .Select(e => e.Value)
                            .DefaultIfEmpty()
                            .Average();

                        return labelWithValues;
                    });

            var labels = await labelsWithValueTask.ToList();
            return labels;
        }

        public async Task<LabelDto> AddAsync(LabelAddDto dto)
        {
            _unitOfWork.BeginTransaction();

            var modelToAdd = _mapper.Map<LabelModel>(dto);

            var modelAdded = await _labelRepository.AddAsync(modelToAdd);

            var result = await _unitOfWork.CommitAsync();
            if (result <= 0)
            {
                return null;
            }

            return _mapper.Map<LabelDto>(modelAdded);
        }

        public async Task<IEnumerable<LabelDto>> GetAllAsync(long groupId)
        {
            // get all labels
            var labelTask = _labelRepository.GetAllAsyncEnumerable();

            // create query
            var currentlabelTask = labelTask.Where(x => x.GroupId.Equals(groupId));

            // execute database query
            var labels = await currentlabelTask.ToList();

            // map to DTO
            return _mapper.Map<IEnumerable<LabelDto>>(labels);
        }
    }
}
