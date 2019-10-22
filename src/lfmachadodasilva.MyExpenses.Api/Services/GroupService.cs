using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public interface IGroupService : IService<GroupModel, GroupDto>
    {
        Task<IEnumerable<GroupDto>> GetAllAsync(long userId);
    }

    public class GroupService : ServiceBase<GroupModel, GroupDto>, IGroupService
    {
        private readonly IGroupRepository _repository;
        private readonly IMapper _mapper;

        public GroupService(
            IGroupRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
            : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> GetAllAsync(long userId)
        {
            var groups = await _repository
                .GetAllWithAllIncludeAsync()
                .Where(g => g.UserGroups.Any(ug => ug.UserId.Equals(userId)))
                .ToList();

            return groups.Select(x =>
            {
                var group = _mapper.Map<GroupDto>(x);
                group.Users = _mapper.Map<IEnumerable<UserDto>>(x.UserGroups.Select(y => y.User));
                return group;
            });
        }

        public override async Task<GroupDto> GetByIdAsync(long id)
        {
            var model = await _repository.GetByIdAsync(id);

            var dto = _mapper.Map<GroupDto>(model);
            dto.Users = _mapper.Map<IEnumerable<UserDto>>(model.UserGroups.Select(y => y.User));

            return dto;
        }
    }
}
