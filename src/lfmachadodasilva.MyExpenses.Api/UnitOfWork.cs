using System.Threading.Tasks;

namespace lfmachadodasilva.MyExpenses.Api
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begin transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit
        /// </summary>
        Task<int> CommitAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyExpensesContext _context;

        public UnitOfWork(MyExpensesContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            // Method intentionally left empty.
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
