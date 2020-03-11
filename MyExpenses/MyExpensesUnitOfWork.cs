using System;
using System.Threading.Tasks;

namespace MyExpenses
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

    public class MyExpensesUnitOfWork : IUnitOfWork
    {
        private readonly MyExpensesContext _context;

        public MyExpensesUnitOfWork(MyExpensesContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public void BeginTransaction()
        {
            // Method intentionally left empty.
        }

        /// <inheritdoc />
        public Task<int> CommitAsync()
        {
            throw new NotImplementedException();

            // return _context.SaveChangesAsync();
        }
    }
}
