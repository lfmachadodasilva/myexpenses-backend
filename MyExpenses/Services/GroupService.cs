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
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ICollection<GroupGetModel>> GetAllAsync(string userId)
        {
            var models = await _repository.GetAll();
            var modelsFiltered = models.Where(g => g.GroupUser.Any(gu => gu.UserId.Equals(userId)));

            return _mapper.Map<ICollection<GroupGetModel>>(modelsFiltered);
        }

        public async Task<ICollection<GroupGetFullModel>> GetAllFullAsync(string userId)
        {
            var models = await _repository.GetAll();
            var modelsFiltered = models.Where(g => g.GroupUser.Any(gu => gu.UserId.Equals(userId)));

            return _mapper.Map<ICollection<GroupGetFullModel>>(modelsFiltered);
        }
    }
}
