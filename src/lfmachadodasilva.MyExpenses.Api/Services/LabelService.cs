using System;
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
        Task<IEnumerable<LabelWithValuesDto>> GetAllWithValues(long groupId, int month, int year);

        /// <summary>
        /// Get all
        /// </summary>
        /// <param name="groupId">group id</param>
        /// <returns>models</returns>
        Task<IEnumerable<LabelDto>> GetAll(long groupId);
    }

    public class LabelService : ServiceBase<LabelModel, LabelDto>, ILabelService
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IMapper _mapper;

        public LabelService(
            ILabelRepository labelRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(labelRepository, unitOfWork, mapper)
        {
            _labelRepository = labelRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LabelWithValuesDto>> GetAllWithValues(long groupId, int month, int year)
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

        public async Task<IEnumerable<LabelDto>> GetAll(long groupId)
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
