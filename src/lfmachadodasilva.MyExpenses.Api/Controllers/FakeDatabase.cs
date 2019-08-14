using System;
using System.Collections.Generic;
using System.Linq;
using lfmachadodasilva.MyExpenses.Api.Models;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    public class FakeDatabase
    {
        public ICollection<LabelDto> Labels;
        public ICollection<GroupDto> Groups;
        public ICollection<ExpenseDto> Expenses;
        public ICollection<UserDto> Users;

        public FakeDatabase()
        {
            if (Expenses == null || Labels == null || Groups == null)
            {
                Random rnd = new Random();

                Labels = new List<LabelDto>();
                Groups = new List<GroupDto>();
                Expenses = new List<ExpenseDto>();
                Users = new List<UserDto>
                {
                    new UserDto
                    {
                        Id = 0,
                        Name = "UserName0",
                    },
                    new UserDto
                    {
                        Id = 1,
                        Name = "UserName1",
                    },
                    new UserDto
                    {
                        Id = 2,
                        Name = "UserName2",
                    }
                };

                for (var g = 0; g < 5; g++)
                {
                    Groups.Add(new GroupDto
                    {
                        Id = g,
                        Name = $"GroupName{g}",
                        Users = Users
                    });

                    ICollection<LabelDto> labelsTmp = new List<LabelDto>();

                    for (var i = 0; i < 20; i++)
                    {
                        labelsTmp.Add(new LabelWithValuesDto
                        {
                            Id = i,
                            Name = $"LabelName{i}",
                            CurrentValue = rnd.Next(1, 250),
                            AverageValue = rnd.Next(1, 250),
                            LastValue = rnd.Next(1, 250),
                            GroupId = g
                        });
                    }

                    for (var i = 0; i < 60; i++)
                    {
                        var idLabel = rnd.Next(0, 20);

                        Expenses.Add(new ExpenseWithValuesDto
                        {
                            Id = i,
                            Name = $"ExpenseName{i}",
                            Value = rnd.Next(1, 250),
                            //LastValue = rnd.Next(1, 250),
                            //AverageValue = rnd.Next(1, 250),
                            Date = DateTime.Today.AddDays(-rnd.Next(1, 60)),

                            LabelId = labelsTmp.ElementAt(idLabel).Id,
                            LabelName = labelsTmp.ElementAt(idLabel).Name,

                            Type = (ExpenseType)rnd.Next(0, 2),

                            GroupId = g
                        });
                    }
                    foreach (var dto in labelsTmp)
                    {
                        Labels.Add(dto);
                    }
                }
            }
        }
    }

    public static class FakeModelDatabase
    {
        public static ICollection<LabelModel> Labels;
        public static ICollection<GroupModel> Groups;
        public static ICollection<ExpenseModel> Expenses;
        public static ICollection<UserModel> Users;
        public static ICollection<UserGroupModel> UserGroups;

        public static void Create()
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
}
