using AutoMapper;
using MyExpenses.Models;

namespace MyExpenses.Repositories
{
    public interface ILabelRepository : IRepository<LabelModel, long>
    {
    }

    public class LabelRepository : RepositoryBase<LabelModel, long>, ILabelRepository
    {
        public LabelRepository(MyExpensesContext context, IMapper mapper) :
            base(context, mapper)
        {
        }
    }
    // public class LabelRepository : ILabelRepository
    // {
    //     private readonly MyExpensesContext _context;
    //     private readonly IMapper _mapper;
    //     private readonly ILogger<LabelRepository> _logger;

    //     public LabelRepository(MyExpensesContext context, IMapper mapper, ILogger<LabelRepository> logger)
    //     {
    //         _context = context;
    //         _mapper = mapper;
    //         _logger = logger;
    //     }

    //     public Task<List<LabelModel>> GetAll()
    //     {
    //         return _context.Labels.ToListAsync();
    //     }

    //     public Task<List<LabelModel>> GetAllWithDetails()
    //     {
    //         return _context.Labels.Include(x => x.Group).ToListAsync();
    //     }

    //     public Task<LabelModel> GetByIdAsync(long id)
    //     {
    //         return _context.Labels.Include(x => x.Group).FirstOrDefaultAsync(x => x.Id.Equals(id));
    //     }

    //     public async Task<LabelModel> UpdateAsync(LabelModel label)
    //     {
    //         if (label == null)
    //         {
    //             _logger.LogInformation("update: null object to update");
    //             return null;
    //         }

    //         var existModel = await GetByIdAsync(label.Id);
    //         if (existModel == null)
    //         {
    //             _logger.LogInformation($"update: {label.Id} does not exists");
    //             return null;
    //         }

    //         // copy attributes
    //         _mapper.Map(label, existModel);

    //         _logger.LogInformation($"updated: {label.Id}");

    //         return existModel;
    //     }
    // }
}
