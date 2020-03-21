using System.Collections.Generic;
using System.Threading.Tasks;
using MyExpenses.Models.Domains;
using MyExpenses.Repositories;

namespace MyExpenses.Services
{
    public interface IGroupService
    {
        /// <summary>
        /// Validate if the user belongs to the group
        /// </summary>
        /// <param name="groupId">Group identification</param>
        /// <param name="userId">User identification</param>
        /// <returns>True if the user can access this group and false otherwise</returns>
        Task<bool> Validate(long groupId, string userId);

        /// <summary>
        /// Get all groups
        /// </summary>
        /// <returns>Collection of groups</returns>
        Task<ICollection<GroupDomain>> GetAllAsync();

        /// <summary>
        /// Get all groups with details
        /// </summary>
        /// <returns>Collection of groups</returns>
        Task<ICollection<GroupDetailsDomain>> GetAllWithDetailsAsync();

        /// <summary>
        /// Get group by id
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Group with detail</returns>
        Task<GroupDetailsDomain> GetByIdAsync(string id);

        /// <summary>
        /// Add new group
        /// </summary>
        /// <param name="domain">Group to add</param>
        /// <returns>Group added</returns>
        Task<GroupDomain> AddAsync(GroupDomain domain);

        /// <summary>
        /// Update group
        /// </summary>
        /// <param name="domain">Group to update</param>
        /// <returns>Group updated</returns>
        Task<GroupDomain> UpdateAsync(GroupDomain domain);

        /// <summary>
        /// Group to delete
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>True if was success and false otherwise</returns>
        Task<bool> DeleteAsync(string id);
    }

    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupRepository">Group repository</param>
        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        /// <inheritdoc />
        public async Task<bool> Validate(long groupId, string userId)
        {
            if (groupId <= 0 || string.IsNullOrEmpty(userId))
            {
                return false;
            }

            return await _groupRepository.Validate(groupId, userId);
        }

        /// <inheritdoc />
        public Task<ICollection<GroupDomain>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<ICollection<GroupDetailsDomain>> GetAllWithDetailsAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<GroupDetailsDomain> GetByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<GroupDomain> AddAsync(GroupDomain domain)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<GroupDomain> UpdateAsync(GroupDomain domain)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
