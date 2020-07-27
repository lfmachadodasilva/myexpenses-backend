using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyExpenses.Helpers;
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
        Task<List<TModel>> GetAllAsync(params Expression<Func<TModel, object>>[] includes);

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns>objects</returns>
        IQueryable<TModel> GetAll();

        /// <summary>
        /// Check if exist
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>true if exist and false otherwise</returns>
        Task<bool> ExistAsync(TModelType id);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="include">doesnt used by base classes</param>
        /// <returns>objects</returns>
        Task<TModel> GetByIdAsync(TModelType id, bool include = false);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model">model to be updated</param>
        /// <param name="user">user id <see href="ICreatedUpdatedModel" /></param>
        /// <returns>model updated</returns>
        Task<TModel> UpdateAsync(TModel model, string user);
        Task<TModel> UpdateAsync(TModel from, TModel to, string user);

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="model">model to be added</param>
        /// <param name="user">user id <see href="ICreatedUpdatedModel" /></param>
        /// <returns>model added</returns>
        Task<TModel> AddAsync(TModel model, string user);

        /// <summary>
        /// Add or update
        /// </summary>
        /// <param name="model">model to be added or updated</param>
        /// <param name="user">user id <see href="ICreatedUpdatedModel" /></param>
        /// <returns>model added or updated</returns>
        Task<TModel> AddOrUpdateAsync(TModel model, string user = null);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="user">user id, only allow to delete if was created by</param>
        /// <returns>True if was removed and false otherwise</returns>
        Task<bool> DeleteAsync(TModelType id, string user);
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
        public virtual Task<List<TModel>> GetAllAsync(params Expression<Func<TModel, object>>[] includes)
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
        public virtual IQueryable<TModel> GetAll()
        {
            //_logger.LogInformation("get all");

            IQueryable<TModel> models = _context.Set<TModel>();
            return models;
        }

        /// <inheritdoc />
        public virtual Task<bool> ExistAsync(TModelType id)
        {
            //_logger.LogInformation($"get by id: {id}");

            IQueryable<TModel> models = _context.Set<TModel>();

            return models.AnyAsync(x => x.Id.Equals(id));
        }

        /// <inheritdoc />
        public virtual Task<TModel> GetByIdAsync(TModelType id, bool include = false)
        {
            //_logger.LogInformation($"get by id: {id}");

            IQueryable<TModel> models = _context.Set<TModel>();

            return models.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        /// <inheritdoc />
        public virtual async Task<TModel> UpdateAsync(TModel model, string user)
        {
            if (model == null)
            {
                //_logger.LogInformation("update: null object to update");
                return null;
            }

            var existModel = await GetByIdAsync(model.Id, false);
            if (existModel == null)
            {
                //_logger.LogInformation($"update: {model.Id} does not exists");
                throw new KeyNotFoundException();
            }

            if (model is IModelValidate modelValidate)
            {
                if (modelValidate.CheckIfIsForbidden(user))
                {
                    throw new ForbidException("FORBIT_ACCESS");
                }
            }

            if (model is ICreatedUpdatedModel createdUpdated)
            {
                createdUpdated.Updated = DateTime.Now;
                createdUpdated.UpdatedById = user;
                // change back
                model = createdUpdated as TModel;
            }

            // copy attributes
            _mapper.Map<TModel, TModel>(model, existModel);

            return existModel;
        }

        /// <inheritdoc />
        public virtual async Task<TModel> UpdateAsync(TModel from, TModel to, string user)
        {
            if (from == null)
            {
                //_logger.LogInformation($"update: {model.Id} does not exists");
                throw new KeyNotFoundException();
            }

            if (from is IModelValidate modelValidate)
            {
                if (modelValidate.CheckIfIsForbidden(user))
                {
                    throw new ForbidException("FORBIT_ACCESS");
                }
            }

            if (from is ICreatedUpdatedModel createdUpdated)
            {
                createdUpdated.Updated = DateTime.Now;
                createdUpdated.UpdatedById = user;
                // change back
                from = createdUpdated as TModel;
            }

            // copy attributes
            _mapper.Map<TModel, TModel>(from, to);

            return to;
        }

        /// <inheritdoc />
        public virtual async Task<TModel> AddAsync(TModel model, string user)
        {
            if (model == null)
            {
                //_logger.LogInformation("add: null object to update");
                return null;
            }

            if (model is IModelValidate modelValidate)
            {
                if (modelValidate.CheckIfIsForbidden(user))
                {
                    throw new ForbidException("FORBIT_ACCESS");
                }
            }

            if (model is ICreatedUpdatedModel createdUpdated)
            {
                createdUpdated.Created = DateTime.Now;
                createdUpdated.UpdatedById = user;
                // change back
                model = createdUpdated as TModel;
            }

            var models = _context.Set<TModel>();
            var newModel = await models.AddAsync(model);

            //_logger.LogInformation($"added: {newModel.Entity.Id}");

            return newModel.Entity;
        }

        /// <inheritdoc />
        public virtual async Task<TModel> AddOrUpdateAsync(TModel model, string user = null)
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
                return await AddAsync(model, user);
            }

            if (string.IsNullOrEmpty(user) && model is IModelValidate modelValidate)
            {
                if (modelValidate.CheckIfIsForbidden(user))
                {
                    throw new ForbidException("FORBIT_ACCESS");
                }
            }

            if (model is ICreatedUpdatedModel createdUpdated)
            {
                createdUpdated.Updated = DateTime.Now;
                if (string.IsNullOrEmpty(user))
                {
                    createdUpdated.UpdatedById = user;
                }
                // change back
                model = createdUpdated as TModel;
            }

            // copy attributes
            _mapper.Map(model, existModel);

            //_logger.LogInformation($"updated: {model.Id}");

            return existModel;
        }

        /// <inheritdoc />
        public virtual async Task<bool> DeleteAsync(TModelType id, string user)
        {
            TModel model = await GetByIdAsync(id);
            if (model == null)
            {
                //_logger.LogInformation($"delete: {id} does not exists");
                throw new KeyNotFoundException();
            }
            if (model is IModelValidate modelValidate)
            {
                if (modelValidate.CheckIfIsForbidden(user))
                {
                    throw new ForbidException("FORBIT_ACCESS");
                }
            }
            if (model is ICreatedUpdatedModel createdUpdated)
            {
                if (!createdUpdated.CreatedById.Equals(user))
                {
                    throw new ForbidException("FORBIT_DELETE");
                }
            }

            var result = _context.Remove(model) != null;
            //_logger.LogInformation($"removed: {id}");

            return result;
        }
    }
}