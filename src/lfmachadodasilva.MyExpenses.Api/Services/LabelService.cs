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
        public Task<IEnumerable<LabelWithValuesDto>> GetAllWithValues(long groupId, int month, int year)
        {
            return Task.Run(() =>
            {
                var labels = _labelRepository
                    .GetAll(x => x.Expenses)
                    .Select(label =>
                    {
                        var labelWithValues = _mapper.Map<LabelWithValuesDto>(label);

                        labelWithValues.CurrentValue = label.Expenses
                            .Where(e =>
                                e.GroupId.Equals(groupId) &&
                                e.Date.Month.Equals(month) &&
                                e.Date.Year.Equals(year))
                            .Sum(x => x.Value);

                        var currentDate = new DateTime(year, month, 1);
                        var lastMonth = currentDate.AddMonths(-1);

                        labelWithValues.LastValue = label.Expenses
                            .Where(e =>
                                e.GroupId.Equals(groupId) &&
                                e.Date.Month.Equals(lastMonth.Month) &&
                                e.Date.Year.Equals(lastMonth.Year))
                            .Sum(x => x.Value);

                        labelWithValues.AverageValue = label.Expenses
                            .Where(e =>
                                e.GroupId.Equals(groupId) &&
                                e.Date < currentDate)
                            .Average(x => x.Value);

                        return labelWithValues;
                    });

                return labels;
            });
        }
    }
}
