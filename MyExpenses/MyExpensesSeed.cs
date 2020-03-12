using System;
using System.Collections.Generic;
using System.Linq;
using MyExpenses.Models;

namespace MyExpenses
{
    public class MyExpensesSeed
    {
        private readonly MyExpensesContext _context;

        public MyExpensesSeed(MyExpensesContext context)
        {
            _context = context;
        }

        public void Run()
        {
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
                    //new UserModel
                    //{
                    //    //Id = 1,
                    //    DisplayName = "UserName1",
                    //},
                    //new UserModel
                    //{
                    //    //Id = 2,
                    //    DisplayName = "UserName2",
                    //},
                    //new UserModel
                    //{
                    //    //Id = 3,
                    //    DisplayName = "UserName3",
                    //},
                    new UserModel
                    {
                        Id = "prCSRxTzTyRjaeDr9SzlvY6gAEi2",
                        Email = "silvaaavlis@gmail.com"
                    },
                    new UserModel
                    {
                        Id = "13FAoQ4yNNSl7mUJtQgTQpFeWmU2",
                        Email = "user@test.com"
                    }
                };

            var result = new List<UserModel>();
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

            var result = new List<GroupModel>();
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
                var userGroup = new GroupUserModel
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
            var rnd = new Random();

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