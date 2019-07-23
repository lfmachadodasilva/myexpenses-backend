using Microsoft.EntityFrameworkCore;

namespace lfmachadodasilva.MyExpenses.Api
{
    public class MyExpensesContext : DbContext
    {
        public MyExpensesContext(DbContextOptions<MyExpensesContext> options)
            : base(options)
        {
        }
    }
}
