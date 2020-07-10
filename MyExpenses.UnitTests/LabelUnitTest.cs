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
    public class LabelUnitTests : UnitTestBase
    {
        private LabelController _controller;

        public LabelUnitTests()
        {
            AddUsers();
            AddGroups();
            AddLabels();

            _controller = ServiceProvider.GetService<LabelController>();
            MockUser(_controller, DefaultUser);
        }

        [Fact]
        public async Task Label_GetAll_ShouldReturnData()
        {
            var results = await _controller.GetAll(DefaultGroup);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<LabelManageModel>>()
                .Which.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Label_GetAllWithInvalidUser_ShouldNotReturnData()
        {
            MockUser(_controller, DefaultInvalidUser);

            var results = await _controller.GetAll(DefaultGroup);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<LabelManageModel>>()
                .Which.Should().BeEmpty();
        }

        [Fact]
        public async Task Label_GetAllWithInvalidGroup_ShouldNotReturnData()
        {
            var results = await _controller.GetAll(DefaultInvalidGroup);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<ICollection<LabelManageModel>>()
                .Which.Should().BeEmpty();
        }

        // [Fact]
        // public async Task Label_GetAllFull_ShouldReturnData()
        // {
        //     var results = await _controller.GetAllFull(DefaultGroup, DefaultMonth, DefaultYear);

        //     results
        //         .Should().BeOfType<OkObjectResult>()
        //         .Which.Value.Should().BeAssignableTo<ICollection<LabelGetFullModel>>()
        //         .Which.Should().NotBeEmpty();
        // }

        // [Fact]
        // public async Task Label_GetAllFullWithInvalidUser_ShouldNotReturnData()
        // {
        //     MockUser(_controller, DefaultInvalidUser);

        //     var results = await _controller.GetAllFull(DefaultGroup, DefaultMonth, DefaultYear);

        //     results
        //         .Should().BeOfType<OkObjectResult>()
        //         .Which.Value.Should().BeAssignableTo<ICollection<LabelGetFullModel>>()
        //         .Which.Should().BeEmpty();
        // }

        // [Fact]
        // public async Task Label_GetAllFullWithInvalidGroup_ShouldNotReturnData()
        // {
        //     var results = await _controller.GetAllFull(DefaultInvalidGroup, DefaultMonth, DefaultYear);

        //     results
        //         .Should().BeOfType<OkObjectResult>()
        //         .Which.Value.Should().BeAssignableTo<ICollection<LabelGetFullModel>>()
        //         .Which.Should().BeEmpty();
        // }

        [Fact]
        public async Task Label_Get_ShouldReturnData()
        {
            var results = await _controller.Get(DefaultLabel);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<LabelManageModel>()
                .Which.Should().NotBeNull();
        }

        [Fact]
        public async Task Label_GetWithInvalidLabel_ShouldReturnNotFount()
        {
            var results = await _controller.Get(DefaultInvalidLabel);

            results.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Label_GetWithInvalidUser_ShouldReturnForbid()
        {
            MockUser(_controller, DefaultInvalidUser);

            var results = await _controller.Get(DefaultLabel);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Label_Post_ShouldReturnData()
        {
            var model = new LabelAddModel
            {
                Name = "New label",
            };
            var results = await _controller.Post(DefaultGroup, model);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<LabelManageModel>()
                .Which.Should().NotBeNull();
        }

        [Fact]
        public async Task Label_PostWithInvalidUser_ShouldReturnForbid()
        {
            MockUser(_controller, DefaultInvalidUser);

            var model = new LabelAddModel
            {
                Name = "New label"
            };
            var results = await _controller.Post(DefaultGroup, model);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Label_PostWithInvalid_ShouldReturnForbid()
        {
            var model = new LabelAddModel
            {
                Name = "New label"
            };
            var results = await _controller.Post(DefaultInvalidGroup, model);

            results.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Label_PostWithInvalid_ShouldReturnBadRequest()
        {
            var results = await _controller.Post(DefaultGroup, null);

            results.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Label_Put_ShouldReturnData()
        {
            var model = new LabelManageModel
            {
                Id = DefaultLabel,
                Name = "New name"
            };
            var results = await _controller.Put(model);

            results
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<LabelManageModel>()
                .Which.Should().NotBeNull();
        }

        [Fact]
        public async Task Label_PutWithInvalidLabel_ShouldNotFound()
        {
            var model = new LabelManageModel
            {
                Id = DefaultInvalidLabel,
                Name = "New name"
            };
            var results = await _controller.Put(model);

            results.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Label_PutWithInvalidUser_ShouldForbid()
        {
            MockUser(_controller, DefaultInvalidUser);

            var model = new LabelManageModel
            {
                Id = DefaultLabel,
                Name = "New name"
            };
            var results = await _controller.Put(model);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Label_PutWithInvalidLabel2_ShouldForbid()
        {
            var model = new LabelManageModel
            {
                Id = DefaultLabelOtherGroup,
                Name = "New name"
            };
            var results = await _controller.Put(model);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Label_PutWithInvalidLabel3_ShouldForbid()
        {
            var results = await _controller.Put(null);

            results.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Label_Delete_ShouldReturnData()
        {
            var results = await _controller.Delete(DefaultLabel);

            results.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Label_DeleteWithInvalidGroup_ShouldReturnNotFound()
        {
            var results = await _controller.Delete(DefaultInvalidLabel);

            results.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Label_DeleteWithInvalidUser_ShouldReturnForbid()
        {
            MockUser(_controller, DefaultInvalidUser);

            var results = await _controller.Delete(DefaultLabel);

            results.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task Label_DeleteWithInvalidGroup_ShouldReturnForbid()
        {
            var results = await _controller.Delete(DefaultLabelOtherGroup);

            results.Should().BeOfType<ForbidResult>();
        }
    }
}