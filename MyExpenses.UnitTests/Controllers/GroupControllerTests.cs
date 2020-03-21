using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyExpenses.Controllers;
using MyExpenses.Models.Domains;
using MyExpenses.Models.Dtos;
using MyExpenses.Services;
using Xunit;

namespace MyExpenses.UnitTests.Controllers
{
    public class GroupControllerTests
    {
        private readonly GroupController _groupController;
        private readonly Mock<IGroupService> _groupServiceMock;
        private readonly Fixture _fixture;
        private readonly IMapper _mapper;

        public GroupControllerTests()
        {
            _groupServiceMock = new Mock<IGroupService>();
            _fixture = new Fixture();

            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<MyExpensesProfile>(); });
            _mapper = mapperConfig.CreateMapper();

            _groupController = new GroupController(_groupServiceMock.Object, _mapper);
        }


        [Fact]
        public async Task GroupController_GetAll_ShouldReturnData()
        {
            // Arrange
            _groupServiceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(_fixture.CreateMany<GroupDomain>().ToList());

            // Act
            var actual = await _groupController.Get();

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<List<GroupDto>>()
                .Which.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GroupController_GetWithDetails_ShouldReturnData()
        {
            // Arrange
            _groupServiceMock
                .Setup(x => x.GetAllWithDetailsAsync())
                .ReturnsAsync(_fixture.CreateMany<GroupDetailsDomain>().ToList());

            // Act
            var actual = await _groupController.GetWithDetails();

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<List<GroupDetailsDto>>()
                .Which.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GroupController_GetById_ShouldReturnData()
        {
            // Arrange
            _groupServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Create<GroupDetailsDomain>());

            // Act
            var actual = await _groupController.Get("ID");

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<GroupDetailsDto>()
                .Which.Should().NotBeNull();
        }

        [Fact]
        public async Task GroupController_Add_ShouldReturnData()
        {
            var userToAdd = _fixture.Create<GroupDto>();

            // Arrange
            _groupServiceMock
                .Setup(x => x.AddAsync(It.IsAny<GroupDomain>()))
                .ReturnsAsync(_mapper.Map<GroupDomain>(userToAdd));

            // Act
            var actual = await _groupController.Post(userToAdd);

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<GroupDto>()
                .Which.Should().BeEquivalentTo(userToAdd);
        }

        [Fact]
        public async Task GroupController_Update_ShouldReturnData()
        {
            var userToAdd = _fixture.Create<GroupDto>();

            // Arrange
            _groupServiceMock
                .Setup(x => x.UpdateAsync(It.IsAny<GroupDomain>()))
                .ReturnsAsync(_mapper.Map<GroupDomain>(userToAdd));

            // Act
            var actual = await _groupController.Put(userToAdd);

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<GroupDto>()
                .Which.Should().BeEquivalentTo(userToAdd);
        }
        [Fact]

        public async Task GroupController_Delete_ShouldReturnData()
        {
            const string userToDelete = "ID";

            // Arrange
            _groupServiceMock
                .Setup(x => x.DeleteAsync(userToDelete))
                .ReturnsAsync(true);

            // Act
            var actual = await _groupController.Delete(userToDelete);

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<bool>()
                .Which.Should().Be(true);
        }
    }
}
