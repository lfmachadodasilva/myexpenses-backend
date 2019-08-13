using lfmachadodasilva.MyExpenses.Api.Controllers;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace lfmachadodasilva.MyExpenses.Api
{
    public interface IMyExpensesSeed
    {
        void Run();
    }

    public class MyExpensesSeed : IMyExpensesSeed
    {
        private readonly MyExpensesContext _context;
        private readonly IWebSettings _webSettings;

        public MyExpensesSeed(MyExpensesContext context, IWebSettings webSettings)
        {
            _context = context;
            _webSettings = webSettings;
        }

        public void Run()
        {
            if (!_webSettings.ClearDatabaseAndSeedData)
            {
                return;
            }

            _context.Database.EnsureDeleted();
            _context.Database.Migrate();

            FakeModelDatabase.Create();

            foreach (var user in FakeModelDatabase.Users)
            {
                _context.Add(user);
            }

            foreach (var group in FakeModelDatabase.Groups)
            {
                _context.Add(group);
            }

            foreach (var userGroup in FakeModelDatabase.UserGroups)
            {
                _context.Add(userGroup);
            }

            foreach (var label in FakeModelDatabase.Labels)
            {
                _context.Add(label);
            }

            foreach (var expense in FakeModelDatabase.Expenses)
            {
                _context.Add(expense);
            }

            _context.SaveChanges();
        }
    }
}
