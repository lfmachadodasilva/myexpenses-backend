using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!_webSettings.UseInMemoryDatabase)
            {
                _context.Database.Migrate();
            }

            var users = AddUsers();
            var groups = AddGroups();
            AddUserGroup(users, groups);

            var labels = AddLabels(groups);
            AddExpenses(groups, labels);
        }

        private IEnumerable<UserModel> AddUsers()
        {
            var models = new List<UserModel>
                {
                    new UserModel
                    {
                        //Id = 1,
                        Name = "UserName1",
                    },
                    new UserModel
                    {
                        //Id = 2,
                        Name = "UserName2",
                    },
                    new UserModel
                    {
                        //Id = 3,
                        Name = "UserName3",
                    }
                };

            List<UserModel> result = new List<UserModel>();
            models.ForEach(model =>
            {
                result.Add(_context.Add(model).Entity);
                _context.SaveChanges();
            });

            return result;
        }

        private IEnumerable<GroupModel> AddGroups()
        {
            var models = new List<GroupModel>
                {
                    new GroupModel
                    {
                        //Id = 1,
                        Name = "GroupName1",
                    },
                    new GroupModel
                    {
                        //Id = 2,
                        Name = "GroupName2",
                    },
                    new GroupModel
                    {
                        //Id = 3,
                        Name = "GroupName3",
                    }
                };

            List<GroupModel> result = new List<GroupModel>();
            models.ForEach(model =>
            {
                result.Add(_context.Add(model).Entity);
                _context.SaveChanges();
            });

            return result;
        }

        private void AddUserGroup(IEnumerable<UserModel> users, IEnumerable<GroupModel> groups)
        {
            var user = users.FirstOrDefault();
            foreach (var group in groups)
            {
                var userGroup = new UserGroupModel
                {
                    UserId = user.Id,
                    GroupId = group.Id
                };
                _context.Add(userGroup);
                var result = _context.SaveChanges();
                //Console.WriteLine($"result: {result}");
            }
        }

        private IEnumerable<LabelModel> AddLabels(IEnumerable<GroupModel> groups)
        {
            var result = new List<LabelModel>();

            foreach (var group in groups)
            {
                for (var i = 1; i < 20; i++)
                {
                    result.Add(_context.Add(new LabelModel
                    {
                        //Id = i,
                        Name = $"LabelName{i}",
                        GroupId = group.Id
                    }).Entity);
                    _context.SaveChanges();
                }
            }

            return result;
        }

        private IEnumerable<ExpenseModel> AddExpenses(IEnumerable<GroupModel> groups, IEnumerable<LabelModel> labels)
        {
            var result = new List<ExpenseModel>();
            Random rnd = new Random();

            foreach (var group in groups)
            {
                var labelsByGroup = labels.Where(x => x.GroupId.Equals(group.Id));

                for (var i = 1; i < 60; i++)
                {
                    var idLabel = rnd.Next(1, 19);

                    result.Add(_context.Add(new ExpenseModel
                    {
                        //Id = i,
                        Name = $"ExpenseName{i}",
                        Value = rnd.Next(1, 250),
                        Date = DateTime.Today.AddDays(-rnd.Next(1, 60)),
                        LabelId = labelsByGroup.ElementAt(idLabel).Id,
                        Type = (ExpenseType)rnd.Next(0, 2),
                        GroupId = group.Id
                    }).Entity);
                    _context.SaveChanges();
                }
            }

            return result;
        }
    }
}
