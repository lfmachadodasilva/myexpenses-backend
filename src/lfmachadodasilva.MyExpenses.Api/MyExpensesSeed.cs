using System;
using System.Collections.Generic;
using System.Linq;
using lfmachadodasilva.MyExpenses.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace lfmachadodasilva.MyExpenses.Api
{
    public class FakeModelDatabase
    {
        public ICollection<LabelModel> Labels;
        public ICollection<GroupModel> Groups;
        public ICollection<ExpenseModel> Expenses;
        public ICollection<UserModel> Users;
        public ICollection<UserGroupModel> UserGroups;

        public FakeModelDatabase()
        {
            if (Expenses == null || Labels == null || Groups == null)
            {
                Random rnd = new Random();

                Labels = new List<LabelModel>();
                Groups = new List<GroupModel>();
                Expenses = new List<ExpenseModel>();
                UserGroups = new List<UserGroupModel>
                {
                    new UserGroupModel
                    {
                        GroupId = 1,
                        UserId = 1
                    },
                    new UserGroupModel
                    {
                        GroupId = 1,
                        UserId = 2
                    },
                    new UserGroupModel
                    {
                        GroupId = 1,
                        UserId = 3
                    }
                };

                Users = new List<UserModel>
                {
                    new UserModel
                    {
                        Id = 1,
                        Name = "UserName0",
                    },
                    new UserModel
                    {
                        Id = 2,
                        Name = "UserName1",
                    },
                    new UserModel
                    {
                        Id = 3,
                        Name = "UserName2",
                    }
                };

                for (var g = 1; g < 2; g++)
                {
                    Groups.Add(new GroupModel
                    {
                        Id = g,
                        Name = $"GroupName{g}",
                        //Users = Users
                    });

                    ICollection<LabelModel> labelsTmp = new List<LabelModel>();

                    for (var i = 1; i < 20; i++)
                    {
                        Labels.Add(new LabelModel
                        {
                            Id = i,
                            Name = $"LabelName{i}",
                            GroupId = g
                        });
                    }

                    for (var i = 1; i < 60; i++)
                    {
                        var idLabel = rnd.Next(1, 19);

                        Expenses.Add(new ExpenseModel
                        {
                            Id = i,
                            Name = $"ExpenseName{i}",
                            Value = rnd.Next(1, 250),
                            Date = DateTime.Today.AddDays(-rnd.Next(1, 60)),
                            LabelId = Labels.ElementAt(idLabel).Id,
                            Type = (ExpenseType)rnd.Next(0, 2),
                            GroupId = g
                        });
                    }
                }
            }
        }
    }

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

            var fake = new FakeModelDatabase();

            foreach (var user in fake.Users)
            {
                _context.Add(user);
            }

            foreach (var group in fake.Groups)
            {
                _context.Add(group);
            }

            foreach (var userGroup in fake.UserGroups)
            {
                _context.Add(userGroup);
            }

            foreach (var label in fake.Labels)
            {
                _context.Add(label);
            }

            foreach (var expense in fake.Expenses)
            {
                _context.Add(expense);
            }

            _context.SaveChanges();
        }
    }
}
