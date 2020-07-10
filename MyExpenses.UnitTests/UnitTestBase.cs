using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyExpenses.Controllers;
using MyExpenses.Helpers;
using MyExpenses.Models;
using MyExpenses.Services;

namespace MyExpenses.UnitTests
{
    public abstract class UnitTestBase
    {
        protected readonly IServiceProvider ServiceProvider;

        protected readonly string DefaultUser = "user1";
        protected readonly string DefaultInvalidUser = "invalid";
        protected readonly long DefaultGroup = 1;
        protected readonly long DefaultInvalidGroup = 100;
        protected readonly long DefaultLabel = 1;
        protected readonly long DefaultLabelOtherGroup = 4;
        protected readonly long DefaultInvalidLabel = 100;
        protected readonly int DefaultMonth = 1;
        protected readonly int DefaultYear = 2020;

        protected UnitTestBase()
        {
            // create a empty configuration file
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.UnitTest.json", optional: true).Build();

            var services = new ServiceCollection()
                .AddMyExpenses(config)
                // add controllers
                .AddTransient<GroupController>()
                .AddTransient<LabelController>()
                .AddTransient<UserController>()
                // automapper
                .AddAutoMapper(typeof(MyExpensesProfile));

            // create service collection
            ServiceProvider = services.BuildServiceProvider();

            // seed data
            var context = ServiceProvider.GetService<MyExpensesContext>();
            var unitOfWork = ServiceProvider.GetService<IUnitOfWork>();

            context.Database.EnsureDeleted();
        }

        protected void AddUsers()
        {
            var context = ServiceProvider.GetService<MyExpensesContext>();
            var unitOfWork = ServiceProvider.GetService<IUnitOfWork>();

            unitOfWork.BeginTransaction();
            context.Add(new UserModel { Id = DefaultUser, Email = "user1@email.com", DisplayName = "User 1" });
            context.Add(new UserModel { Id = "user2", Email = "user2@email.com", DisplayName = "User 2" });
            context.Add(new UserModel { Id = "user3", Email = "user3@email.com", DisplayName = "User 3" });
            unitOfWork.CommitAsync().Wait();
        }

        protected void AddGroups()
        {
            var context = ServiceProvider.GetService<MyExpensesContext>();
            var unitOfWork = ServiceProvider.GetService<IUnitOfWork>();

            unitOfWork.BeginTransaction();
            context.Add(new GroupModel
            {
                Id = DefaultGroup,
                Name = "Group 1",
                GroupUser = new List<GroupUserModel> {
                    new GroupUserModel
                    {
                        GroupId = DefaultGroup,
                        UserId = DefaultUser
                    },
                    new GroupUserModel
                    {
                        GroupId = DefaultGroup,
                        UserId = "user2"
                    }
                }
            });
            context.Add(new GroupModel
            {
                Id = 2,
                Name = "Group 2",
                GroupUser = new List<GroupUserModel> {
                    new GroupUserModel
                    {
                        GroupId = 2,
                        UserId = "user2"
                    },
                    new GroupUserModel
                    {
                        GroupId = 2,
                        UserId = "user3"
                    }
                }
            });
            context.Add(new GroupModel
            {
                Id = 3,
                Name = "Group 3",
                GroupUser = new List<GroupUserModel> {
                    new GroupUserModel
                    {
                        GroupId = 3,
                        UserId = DefaultUser
                    },
                    new GroupUserModel
                    {
                        GroupId = 3,
                        UserId = "user3"
                    }
                }
            });
            unitOfWork.CommitAsync().Wait();
        }

        protected void AddLabels()
        {
            var context = ServiceProvider.GetService<MyExpensesContext>();
            var unitOfWork = ServiceProvider.GetService<IUnitOfWork>();

            unitOfWork.BeginTransaction();

            context.Add(new LabelModel
            {
                Id = 1,
                Name = "Label 1",
                GroupId = DefaultGroup
            });
            context.Add(new LabelModel
            {
                Id = 2,
                Name = "Label 2",
                GroupId = DefaultGroup
            });
            context.Add(new LabelModel
            {
                Id = 3,
                Name = "Label 3",
                GroupId = DefaultGroup
            });

            context.Add(new LabelModel
            {
                Id = 4,
                Name = "Label 4",
                GroupId = 2
            });
            context.Add(new LabelModel
            {
                Id = 5,
                Name = "Label 5",
                GroupId = 2
            });

            context.Add(new LabelModel
            {
                Id = 6,
                Name = "Label 6",
                GroupId = 3
            });

            unitOfWork.CommitAsync().Wait();
        }

        protected void AddExpenses()
        {
            var context = ServiceProvider.GetService<MyExpensesContext>();
            var unitOfWork = ServiceProvider.GetService<IUnitOfWork>();

            unitOfWork.BeginTransaction();

            context.Add(new ExpenseModel
            {
                Id = 1,
                Name = "Expense 1",
                GroupId = DefaultGroup,
                LabelId = DefaultLabel,
                Value = 1,
                Type = ExpenseType.Outcoming,
                Date = DateTime.Today
            });
            context.Add(new ExpenseModel
            {
                Id = 2,
                Name = "Expense 2",
                GroupId = DefaultGroup,
                LabelId = DefaultLabel,
                Value = 2,
                Type = ExpenseType.Incoming,
                Date = DateTime.Today
            });
            context.Add(new ExpenseModel
            {
                Id = 1,
                Name = "Expense 1",
                GroupId = DefaultGroup,
                LabelId = DefaultLabel,
                Value = 1,
                Type = ExpenseType.Outcoming,
                Date = DateTime.Today.AddMonths(-1)
            });
            context.Add(new ExpenseModel
            {
                Id = 2,
                Name = "Expense 2",
                GroupId = DefaultGroup,
                LabelId = DefaultLabel,
                Value = 2,
                Type = ExpenseType.Incoming,
                Date = DateTime.Today.AddMonths(-1)
            });
            unitOfWork.CommitAsync().Wait();
        }

        protected void MockUser(ControllerBase controller, string userId)
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
                            new Claim("user_id", userId)
                        },
                        "mock"))
                }
            };
        }
    }
}
