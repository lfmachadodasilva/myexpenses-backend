using System;
using System.Collections.Generic;
using System.Linq;
using lfmachadodasilva.MyExpenses.Api.Models.Dtos;

namespace lfmachadodasilva.MyExpenses.Api.Controllers
{
    public class FakeDatabase
    {
        public static ICollection<LabelDto> Labels;
        public static ICollection<GroupDto> Groups;
        public static ICollection<ExpenseDto> Expenses;
        public static ICollection<UserDto> Users;

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
                            LastValue = rnd.Next(1, 250),
                            AverageValue = rnd.Next(1, 250),
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
}
