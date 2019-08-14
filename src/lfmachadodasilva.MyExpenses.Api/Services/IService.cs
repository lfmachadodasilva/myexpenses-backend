using System.Collections.Generic;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public interface IService<TModel, TDto> where TModel : IModel where TDto : IDto
    {
        /// <summary>
        /// Get all
        /// </summary>
        /// <returns>dtos</returns>
        Task<IEnumerable<TDto>> GetAllAsync();

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>dto</returns>
        Task<TDto> GetByIdAsync(long id);

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="dto">dto</param>
        /// <returns>dto</returns>
        Task<TDto> AddAsync(TDto dto);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="dto">dto</param>
        /// <returns>dto</returns>
        Task<TDto> UpdateAsync(TDto dto);

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<bool> RemoveAsync(long id);
    }
}
