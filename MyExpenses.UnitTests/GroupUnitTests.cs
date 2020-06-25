using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Controllers;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using MyExpenses.Models;
using System.Security.Claims;
using Moq;

namespace MyExpenses.UnitTests
{
    public class GroupUnitTests : UnitTestBase
    {
        public GroupUnitTests()
        {
            AddUsers();
            AddGroups();
        }

        [Fact]
        public async void Group_GetAll_ShouldReturnData()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.GetAll();

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<GroupGetModel>>()
                .Which.Should().NotBeEmpty();
        }

        [Fact]
        public async void Group_GetAllWithInvalidUser_ShouldNotReturnData()
        {
            ValidateHelperMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsIdentity>()))
                .Returns("invalid");

            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.GetAll();

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<GroupGetModel>>()
                .Which.Should().BeEmpty();
        }

        [Fact]
        public async void Group_GetAllFull_ShouldReturnData()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.GetAllFull();

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<GroupGetFullModel>>()
                .Which.Should().NotBeEmpty();
        }

        [Fact]
        public async void Group_GetAllFullWithInvalidUser_ShouldNotReturnData()
        {
            ValidateHelperMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsIdentity>()))
                .Returns("invalid");

            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.GetAllFull();

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<GroupGetFullModel>>()
                .Which.Should().BeEmpty();
        }
    }
}
