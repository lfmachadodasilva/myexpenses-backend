using System.Collections.Generic;
using System.Threading.Tasks;
using MyExpenses.Models.Domains;

namespace MyExpenses.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>Collection of user</returns>
        Task<ICollection<UserDomain>> GetAllAsync();

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        Task<UserDomain> GetByIdAsync(string id);

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="dto">User to add</param>
        /// <returns>User added</returns>
        Task<UserDomain> AddAsync(UserDomain dto);

        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="dto">User to update</param>
        /// <returns>User updated</returns>
        Task<UserDomain> UpdateAsync(UserDomain dto);

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns>True if was success and false otherwise</returns>
        Task<bool> DeleteAsync(string id);
    }

    public class UserService : IUserService
    {
        /// <inheritdoc />
        public async Task<ICollection<UserDomain>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<UserDomain> GetByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<UserDomain> AddAsync(UserDomain dto)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<UserDomain> UpdateAsync(UserDomain dto)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}
