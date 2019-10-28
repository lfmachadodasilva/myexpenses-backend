using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public interface IUserGroupRepository
    {
        IAsyncEnumerable<UserGroupModel> GetAllAsync();
        Task<UserGroupModel> AddAync(UserGroupModel model);
        Task UpdateAsync(long groupId, IEnumerable<UserGroupModel> models);
    }

    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly MyExpensesContext _context;

        public UserGroupRepository(
            MyExpensesContext context,
            ILogger<UserGroupRepository> logger,
            IMapper mapper)
        {
            _context = context;
        }

        public IAsyncEnumerable<UserGroupModel> GetAllAsync()
        {
            return _context.UserGroup
                //.Include(x => x.Group)
                //.Include(x => x.User)
                .ToAsyncEnumerable();
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

            var toRemove = byGroup
                .Join(
                    models,
                    group => group.UserId,
                    model => model.UserId,
                    (group, model) => group);
            _context.RemoveRange(toRemove);

            var toAdd = models
                .Join(
                    byGroup,
                    group => group.UserId,
                    model => model.UserId,
                    (group, model) => group);
            await _context.AddRangeAsync(toAdd);

            // var result = GetAllAsync().Where(x => x.GroupId.Equals(groupId));
            // return result.ToList();
        }
    }
}