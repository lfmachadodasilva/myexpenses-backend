using System.Threading.Tasks;
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
    }
}
