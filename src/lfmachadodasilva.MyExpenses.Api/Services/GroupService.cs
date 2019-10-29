using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;
using lfmachadodasilva.MyExpenses.Api.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupWithValuesDto>> GetAllAsync(long userId);

        Task<GroupWithValuesDto> GetByIdAsync(long id);

        Task<GroupDto> AddAsync(GroupAddDto dto);

        Task<GroupDto> UpdateAsync(GroupDto dto);
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _repository;
        private readonly IUserGroupRepository _userGrouprepository;
        private readonly IUnitOfWork _unitOfwork;
        private readonly IMapper _mapper;

        public GroupService(
            IGroupRepository repository,
            IUserGroupRepository userGrouprepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _userGrouprepository = userGrouprepository;
            _unitOfwork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupWithValuesDto>> GetAllAsync(long userId)
        {
            var groups = await _repository
                .GetAllWithAllIncludeAsync()
                .Where(g => g.UserGroups.Any(ug => ug.UserId.Equals(userId)))
                .ToList();

            return groups.Select(x =>
            {
                var group = _mapper.Map<GroupWithValuesDto>(x);
                group.Users = _mapper.Map<IEnumerable<UserDto>>(x.UserGroups.Select(y => y.User));
                return group;
            });
        }

        public async Task<GroupWithValuesDto> GetByIdAsync(long id)
        {
            var model = await _repository.GetByIdAsync(id);

            var dto = _mapper.Map<GroupWithValuesDto>(model);
            dto.Users = _mapper.Map<IEnumerable<UserDto>>(model.UserGroups.Select(y => y.User));

            return dto;
        }

        public async Task<GroupDto> AddAsync(GroupAddDto dto)
        {
            _unitOfwork.BeginTransaction();

            var model = _mapper.Map<GroupModel>(dto);

            var modelAdded = await _repository.AddAsync(model);

            var result = await _unitOfwork.CommitAsync();

            if (result < 0)
            {
                // TODO throw
                return null;
            }

            foreach (var item in dto.Users)
            {
                await _userGrouprepository.AddAync(new UserGroupModel
                {
                    GroupId = modelAdded.Id,
                    UserId = item
                });
            }

            result = await _unitOfwork.CommitAsync();

            if (result != dto.Users.Count())
            {
                // TODO throw
                return null;
            }

            var modelAdded2 = await _repository.GetByIdAsync(modelAdded.Id);

            var dtoAdded = _mapper.Map<GroupDto>(modelAdded2);

            return dtoAdded;
        }

        public async Task<GroupDto> UpdateAsync(GroupDto dto)
        {
            _unitOfwork.BeginTransaction();

            var model = _mapper.Map<GroupModel>(dto);

            var groups = dto.Users
                .Select(item => 
                    new UserGroupModel
                    {
                        GroupId = model.Id,
                        UserId = item
                    })
                .ToList();

            await _userGrouprepository.UpdateAsync(model.Id, groups);
            var result = await _unitOfwork.CommitAsync();

            model.UserGroups = await _userGrouprepository.GetAllAsync().Where(x => x.GroupId.Equals(model.Id)).ToList();

            var modelUpdated = await _repository.UpdateAsync(model);

            result = await _unitOfwork.CommitAsync();

            return _mapper.Map<GroupDto>(modelUpdated);
        }
    }
}
