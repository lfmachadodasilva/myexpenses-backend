using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
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
        protected readonly Mock<IValidateHelper> ValidateHelperMock;

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
            ValidateHelperMock = new Mock<IValidateHelper>();
            ValidateHelperMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsIdentity>()))
                .Returns(DefaultUser);

            // create a empty configuration file
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.UnitTest.json", optional: true).Build();

            var services = new ServiceCollection()
                .AddMyExpenses(config)
                // add controllers
                .AddTransient<GroupController>()
                .AddTransient<LabelController>()
                .AddTransient<UserController>()
                // replace by mock
                .AddSingleton<IValidateHelper>(ValidateHelperMock.Object)
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

        protected void MockInvalidUser()
        {
            ValidateHelperMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsIdentity>()))
                .Returns(DefaultInvalidUser);
        }
    }
}
