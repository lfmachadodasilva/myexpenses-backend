using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Controllers;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using MyExpenses.Models;
using System.Threading.Tasks;

namespace MyExpenses.UnitTests
{
    public class UserUnitTests : UnitTestBase
    {
        public UserUnitTests() : base()
        {
            AddUsers();
        }

        [Fact]
        public async Task User_GetAll_ShouldReturnData()
        {
            var controller = ServiceProvider.GetService<UserController>();
            var results = await controller.GetAll();
            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<UserModel>>()
                .Which.Should().NotBeEmpty();
        }

        [Fact]
        public async Task User_PostNewUser_ShouldAdd()
        {
            var newUser = new UserModel
            {
                Id = "user4",
                DisplayName = "User 4",
                Email = "user4@test.com",
                PhotoUrl = "url"
            };
            var controller = ServiceProvider.GetService<UserController>();
            var results = await controller.Post(newUser);
            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<UserModel>()
                .Which.Should().BeEquivalentTo(newUser);
        }

        [Fact]
        public async Task User_PostExistingUser_ShouldUpdate()
        {
            var user = new UserModel
            {
                Id = "user1",
                DisplayName = "New User 1",
                Email = "newuser1@test.com",
                PhotoUrl = "newurl"
            };
            var controller = ServiceProvider.GetService<UserController>();
            var results = await controller.Post(user);
            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<UserModel>()
                .Which.Should().BeEquivalentTo(user);
        }
    }
}
