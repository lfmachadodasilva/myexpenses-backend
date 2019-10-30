using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    /// <summary>
    /// Repository base interface
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRepository<TModel> where TModel : IModel
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns>objects</returns>
        IEnumerable<TModel> GetAll(params Expression<Func<TModel, object>>[] includes);

        /// <summary>
        /// Get all
        /// </summary>
        /// <returns>objects</returns>
        IAsyncEnumerable<TModel> GetAllAsyncEnumerable(params Expression<Func<TModel, object>>[] includes);

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>objects</returns>
        Task<TModel> GetByIdAsync(long id);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model">model to be updated</param>
        /// <returns>model updated</returns>
        Task<TModel> UpdateAsync(TModel model);

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="model">mode to be added</param>
        /// <returns>model added</returns>
        Task<TModel> AddAsync(TModel model);

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>True if was removed and false otherwise</returns>
        Task<bool> RemoveAsync(long id);
    }
}