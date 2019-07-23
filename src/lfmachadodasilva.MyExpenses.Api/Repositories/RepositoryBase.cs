using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public class RepositoryBase<TModel> : IRepository<TModel> where TModel : class, IModel
    {
        private readonly MyExpensesContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RepositoryBase(
            MyExpensesContext context,
            IMapper mapper,
            ILogger logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public virtual IEnumerable<TModel> GetAll()
        {
            _logger.LogInformation("Get all");
            return _context.Set<TModel>();
        }

        public virtual Task<TModel> GetByIdAsync(long id)
        {
            _logger.LogInformation($"Get by id: {id}");
            var models = _context.Set<TModel>();
            return models.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            if (model == null)
                return null;

            TModel existModel = await GetByIdAsync(model.Id);
            if (existModel == null)
                return null;

            // copy attributes
            _mapper.Map(model, existModel);

            _logger.LogInformation($"Updated: {model.Id}");

            return existModel;
        }

        public virtual async Task<TModel> AddAsync(TModel model)
        {
            if (model == null)
                return null;

            var models = _context.Set<TModel>();
            var newModel = await models.AddAsync(model);

            _logger.LogInformation($"add: {newModel.Entity.Id}");

            return newModel.Entity;
        }

        public virtual async Task<bool> RemoveAsync(long id)
        {
            TModel model = await GetByIdAsync(id);
            if (model == null)
                return false;

            _logger.LogInformation($"Remove: {id}");

            return _context.Remove(model) != null;
        }
    }
}
