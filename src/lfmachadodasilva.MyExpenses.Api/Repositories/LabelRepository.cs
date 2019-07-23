using AutoMapper;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.Extensions.Logging;

namespace lfmachadodasilva.MyExpenses.Api.Repositories
{
    public interface ILabelRepository : IRepository<LabelModel>
    {
    }

    public class LabelRepository: RepositoryBase<LabelModel>, ILabelRepository
    {
        public LabelRepository(MyExpensesContext context, IMapper mapper, ILogger logger) :
            base(context, mapper, logger)
        {
        }
    }
}
