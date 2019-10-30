using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    /// <inheritdoc />
    public abstract class RepositoryBase<TModel> : IRepository<TModel> where TModel : class, IModel
    {
        private readonly MyExpensesContext _context;

        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        protected RepositoryBase(MyExpensesContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public virtual IEnumerable<TModel> GetAll(params Expression<Func<TModel, object>>[] includes)
        {
            _logger.LogInformation("get all");
            IQueryable<TModel> models = _context.Set<TModel>();
            foreach (var include in includes)
            {
                models = models.Include(include);
            }
            return models;
        }

        /// <inheritdoc />
        public virtual IAsyncEnumerable<TModel> GetAllAsyncEnumerable(params Expression<Func<TModel, object>>[] includes)
        {
            _logger.LogInformation("get all");

            IQueryable<TModel> models = _context.Set<TModel>();
            foreach (var include in includes)
            {
                models = models.Include(include);
            }

            return models.ToAsyncEnumerable();
        }

        /// <inheritdoc />
        public virtual Task<TModel> GetByIdAsync(long id)
        {
            _logger.LogInformation($"get by id: {id}");

            IQueryable<TModel> models = _context.Set<TModel>();

            return models.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            if (model == null)
            {
                _logger.LogInformation("update: null object to update");
                return null;
            }

            var existModel = await GetByIdAsync(model.Id);
            if (existModel == null)
            {
                _logger.LogInformation($"update: {model.Id} does not exists");
                return null;
            }

            // copy attributes
            _mapper.Map(model, existModel);

            _logger.LogInformation($"updated: {model.Id}");

            return existModel;
        }

        /// <inheritdoc />
        public virtual async Task<TModel> AddAsync(TModel model)
        {
            if (model == null)
            {
                _logger.LogInformation("add: null object to update");
                return null;
            }

            var models = _context.Set<TModel>();
            var newModel = await models.AddAsync(model);

            _logger.LogInformation($"added: {newModel.Entity.Id}");

            return newModel.Entity;
        }

        /// <inheritdoc />
        public virtual async Task<bool> RemoveAsync(long id)
        {
            TModel model = await GetByIdAsync(id);
            if (model == null)
            {
                _logger.LogInformation($"delete: {id} does not exists");
                return false;
            }

            var result = _context.Remove(model) != null;
            _logger.LogInformation($"removed: {id}");

            return result;
        }
    }
}
