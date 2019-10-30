using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public interface IUserGroupRepository
    {
        IAsyncEnumerable<UserGroupModel> GetAllAsync(bool include = false);
        Task<UserGroupModel> AddAync(UserGroupModel model);
        Task UpdateAsync(long groupId, IEnumerable<UserGroupModel> models);
    }

    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly MyExpensesContext _context;

        public UserGroupRepository(
            MyExpensesContext context,
            ILogger<UserGroupRepository> logger)
        {
            _context = context;
        }

        public IAsyncEnumerable<UserGroupModel> GetAllAsync(bool include = false)
        {
            if (include)
            {
                return _context.UserGroup
                    .Include(x => x.Group)
                    .Include(x => x.User)
                    .ToAsyncEnumerable();
            }
            return _context.UserGroup.ToAsyncEnumerable();
        }

        public async Task<UserGroupModel> AddAync(UserGroupModel model)
        {
            var newModel = await _context.UserGroup.AddAsync(model);
            return newModel.Entity;
        }

        public async Task UpdateAsync(long groupId, IEnumerable<UserGroupModel> models)
        {
            var byGroupTask = GetAllAsync().Where(x => x.GroupId.Equals(groupId));
            var byGroup = await byGroupTask.ToList();

            var toRemove = byGroup.Where(x => models.Any(y => !y.UserId.Equals(x.UserId)));
            if (toRemove.Any())
            {
                _context.RemoveRange(toRemove);
            }

            var toAdd = models.Where(x => !byGroup.Any(y => !y.UserId.Equals(x.UserId)));
            if (toAdd.Any())
            {
                await _context.AddRangeAsync(toAdd);
            }

            // var result = GetAllAsync().Where(x => x.GroupId.Equals(groupId));
            // return result.ToList();
        }
    }
}