using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyExpenses.Controllers;
using MyExpenses.Models;

namespace MyExpenses.UnitTests
{
    public abstract class UnitTestBase
    {
        protected readonly IServiceProvider ServiceProvider;

        protected UnitTestBase()
        {
            // create a empty configuration file
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.UnitTest.json", optional: true).Build();

            // create service collection
            ServiceProvider =
                new ServiceCollection()
                    .AddMyExpenses(config)
                    .AddTransient<UserController>()
                    .AddAutoMapper(typeof(MyExpensesProfile))
                    .BuildServiceProvider();

            // seed data
            var context = ServiceProvider.GetService<MyExpensesContext>();
            var unitOfWork = ServiceProvider.GetService<IUnitOfWork>();

            unitOfWork.BeginTransaction();
            context.Database.EnsureDeleted();
            context.Add(new UserModel { Id = "user1", Email = "user1@email.com", DisplayName = "User 1" });
            context.Add(new UserModel { Id = "user2", Email = "user2@email.com", DisplayName = "User 2" });
            context.Add(new UserModel { Id = "user3", Email = "user3@email.com", DisplayName = "User 3" });
            unitOfWork.CommitAsync().Wait();
        }
    }
}
