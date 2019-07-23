using System.Collections.Generic;
using System.Threading.Tasks;
using lfmachadodasilva.MyExpenses.Api.Models;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public interface IRepository<TModel> where TModel : IModel
    {
        IEnumerable<TModel> GetAll();

        Task<TModel> GetByIdAsync(long id);

        Task<TModel> UpdateAsync(TModel model);

        Task<TModel> AddAsync(TModel model);

        Task<bool> RemoveAsync(long id);
    }
}
