using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using MyExpenses.Controllers;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using MyExpenses.Models;
using System.Security.Claims;
using Moq;
using System.Threading.Tasks;

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
        public async Task Group_GetAll_ShouldReturnData()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.GetAll();

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<GroupGetModel>>()
                .Which.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Group_GetAllWithInvalidUser_ShouldNotReturnData()
        {
            MockInvalidUser();

            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.GetAll();

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<GroupGetModel>>()
                .Which.Should().BeEmpty();
        }

        [Fact]
        public async Task Group_GetAllFull_ShouldReturnData()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.GetAllFull();

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<GroupGetFullModel>>()
                .Which.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Group_GetAllFullWithInvalidUser_ShouldNotReturnData()
        {
            MockInvalidUser();

            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.GetAllFull();

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<GroupGetFullModel>>()
                .Which.Should().BeEmpty();
        }

        [Fact]
        public async Task Group_Get_ShouldReturnData()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Get(DefaultGroup);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<GroupGetModel>()
                .Which.Should().NotBeNull();
        }

        [Fact]
        public async Task Group_GetWithInvalidUser_ShouldReturnForbid()
        {
            MockInvalidUser();

            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Get(DefaultGroup);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Group_GetWithInvalidGroup_ShouldReturnNotFound()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Get(DefaultInvalidGroup);

            results.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Group_Post_ShouldReturnData()
        {
            var model = new GroupManageModel
            {
                Name = "New user",
                Users = new List<UserModelBase>() { new UserModelBase { Id = DefaultUser } }
            };
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Post(model);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<GroupManageModel>()
                .Which.Should().NotBeNull();
        }

        [Fact]
        public async Task Group_PostWithInvalidUser_ShouldReturnForbid()
        {
            MockInvalidUser();

            var model = new GroupManageModel
            {
                Name = "New user",
                Users = new List<UserModelBase>() { new UserModelBase { Id = DefaultUser } }
            };
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Post(model);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Group_PostWithInvalidUser2_ShouldReturnForbid()
        {
            var model = new GroupManageModel
            {
                Name = "New user",
                Users = new List<UserModelBase>() { new UserModelBase { Id = DefaultInvalidUser } }
            };
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Post(model);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Group_PostWithInvalidGroup_ShouldReturnBadRequest()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Post(null);

            results.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Group_Put_ShouldReturnData()
        {
            var model = new GroupManageModel
            {
                Id = DefaultGroup,
                Name = "New name",
                Users = new List<UserModelBase>() { new UserModelBase { Id = DefaultUser } }
            };
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Put(model);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<GroupManageModel>()
                .Which.Should().NotBeNull();
        }

        [Fact]
        public async Task Group_PutWithInvalidUser_ShouldReturnForbid()
        {
            MockInvalidUser();

            var model = new GroupManageModel
            {
                Id = DefaultGroup,
                Name = "New user",
                Users = new List<UserModelBase>() { new UserModelBase { Id = DefaultUser } }
            };
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Put(model);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Group_PutWithInvalidUser2_ShouldReturnForbid()
        {
            var model = new GroupManageModel
            {
                Id = DefaultGroup,
                Name = "New user",
                Users = new List<UserModelBase>() { new UserModelBase { Id = DefaultInvalidUser } }
            };
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Put(model);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Group_PutWithInvalidGroup_ShouldReturnNotFound()
        {
            var model = new GroupManageModel
            {
                Id = DefaultInvalidGroup,
                Name = "New user",
                Users = new List<UserModelBase>() { new UserModelBase { Id = DefaultUser } }
            };
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Put(model);

            results.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Group_PutWithInvalidGroup_ShouldReturnBadRequest()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Put(null);

            results.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Group_Delete_ShouldReturnData()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Delete(DefaultGroup);

            results.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Group_DeleteWithInvalidGroup_ShouldReturnNotFound()
        {
            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Delete(DefaultInvalidGroup);

            results.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Group_DeleteWithInvalidUser_ShouldReturnNotFound()
        {
            MockInvalidUser();

            var controller = ServiceProvider.GetService<GroupController>();
            var results = await controller.Delete(DefaultGroup);

            results.Should().BeOfType<ForbidResult>();
        }
    }
}
