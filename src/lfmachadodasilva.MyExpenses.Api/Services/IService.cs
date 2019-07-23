using System.Collections.Generic;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;

namespace lfmachadodasilva.MyExpenses.Api.Services
{
    public interface IService<TDto> where TDto : IDto
    {
        IEnumerable<TDto> GetAll();

        Task<TDto> GetByIdAsync(long id);

        Task<TDto> UpdateAsync(TDto dto);

        Task<TDto> AddAsync(TDto model);

        Task<bool> RemoveAsync(long id);
    }
}
