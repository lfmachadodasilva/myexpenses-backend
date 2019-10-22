using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public interface IGroupRepository : IRepository<GroupModel>
    {
        IAsyncEnumerable<GroupModel> GetAllWithAllIncludeAsync();
    }

    public class GroupRepository : RepositoryBase<GroupModel>, IGroupRepository
    {
        private readonly MyExpensesContext _context;
        private readonly ILogger<GroupRepository> _logger;

        public GroupRepository(
            MyExpensesContext context,
            ILogger<GroupRepository> logger,
            IMapper mapper)
            : base(context, logger, mapper)
        {
            _context = context;
            _logger = logger;
        }

        public IAsyncEnumerable<GroupModel> GetAllWithAllIncludeAsync()
        {
            _logger.LogInformation("get all with all include");

            return _context.Set<GroupModel>()
                .Include(x => x.UserGroups)
                    .ThenInclude(x => x.User)
                    .ThenInclude(x => x.UserGroups)
                .ToAsyncEnumerable();
        }
    }
}
