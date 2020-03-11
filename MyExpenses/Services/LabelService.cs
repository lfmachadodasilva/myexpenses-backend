using System.Collections.Generic;
using System.Threading.Tasks;
using MyExpenses.Models;
using MyExpenses.Repositories;

namespace MyExpenses.Services
{
    public interface ILabelService
    {
        Task<List<LabelModel>> GetAll();
    }

    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepository;

        public LabelService(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public Task<List<LabelModel>> GetAll()
        {
            return _labelRepository.GetAll(x => x.Group);
        }
    }
}
