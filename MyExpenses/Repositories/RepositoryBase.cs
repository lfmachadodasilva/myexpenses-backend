using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    /// <summary>
    /// Repository base interface
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRepository<TModel, TModelType> where TModel : IModel<TModelType>
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns>objects</returns>
        Task<List<TModel>> GetAll(params Expression<Func<TModel, object>>[] includes);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>objects</returns>
        Task<TModel> GetByIdAsync(TModelType id);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model">model to be updated</param>
        /// <returns>model updated</returns>
        Task<TModel> UpdateAsync(TModel model);

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="model">model to be added</param>
        /// <returns>model added</returns>
        Task<TModel> AddAsync(TModel model);

        /// <summary>
        /// Add or update
        /// </summary>
        /// <param name="model">model to be added or updated</param>
        /// <returns>model added or updated</returns>
        Task<TModel> AddOrUpdateAsync(TModel model);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>True if was removed and false otherwise</returns>
        Task<bool> DeleteAsync(TModelType id);
    }

    /// <inheritdoc />
    public abstract class RepositoryBase<TModel, TModelType> : IRepository<TModel, TModelType> where TModel : class, IModel<TModelType>
    {
        private readonly MyExpensesContext _context;
        private readonly IMapper _mapper;

        protected RepositoryBase(MyExpensesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public virtual Task<List<TModel>> GetAll(params Expression<Func<TModel, object>>[] includes)
        {
            //_logger.LogInformation("get all");

            IQueryable<TModel> models = _context.Set<TModel>();
            foreach (var include in includes)
            {
                models = models.Include(include);
            }

            return models.ToListAsync();
        }

        /// <inheritdoc />
        public virtual Task<TModel> GetByIdAsync(TModelType id)
        {
            //_logger.LogInformation($"get by id: {id}");

            IQueryable<TModel> models = _context.Set<TModel>();

            return models.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        /// <inheritdoc />
        public virtual async Task<TModel> UpdateAsync(TModel model)
        {
            if (model == null)
            {
                //_logger.LogInformation("update: null object to update");
                return null;
            }

            var existModel = await GetByIdAsync(model.Id);
            if (existModel == null)
            {
                //_logger.LogInformation($"update: {model.Id} does not exists");
                return null;
            }

            // copy attributes
            _mapper.Map(model, existModel);

            //_logger.LogInformation($"updated: {model.Id}");

            return existModel;
        }

        /// <inheritdoc />
        public virtual async Task<TModel> AddAsync(TModel model)
        {
            if (model == null)
            {
                //_logger.LogInformation("add: null object to update");
                return null;
            }

            var models = _context.Set<TModel>();
            var newModel = await models.AddAsync(model);

            //_logger.LogInformation($"added: {newModel.Entity.Id}");

            return newModel.Entity;
        }

        /// <inheritdoc />
        public virtual async Task<TModel> AddOrUpdateAsync(TModel model)
        {
            if (model == null)
            {
                //_logger.LogInformation("update: null object to update");
                return null;
            }

            var existModel = await GetByIdAsync(model.Id);
            if (existModel == null)
            {
                //_logger.LogInformation($"update: {model.Id} does not exists");
                return await AddAsync(model);
            }

            // copy attributes
            _mapper.Map(model, existModel);

            //_logger.LogInformation($"updated: {model.Id}");

            return existModel;
        }

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(TModelType id)
        {
            TModel model = await GetByIdAsync(id);
            if (model == null)
            {
                //_logger.LogInformation($"delete: {id} does not exists");
                return false;
            }

            var result = _context.Remove(model) != null;
            //_logger.LogInformation($"removed: {id}");

            return result;
        }
    }
}