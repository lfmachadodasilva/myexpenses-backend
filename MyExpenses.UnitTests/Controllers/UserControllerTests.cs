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
    public class UserControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Fixture _fixture;
        private readonly IMapper _mapper;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _fixture = new Fixture();

            var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<MyExpensesProfile>(); });
            _mapper = mapperConfig.CreateMapper();

            _userController = new UserController(_userServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task UserController_GetAll_ShouldReturnData()
        {
            // Arrange
            _userServiceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(_fixture.CreateMany<UserDomain>().ToList());

            // Act
            var actual = await _userController.Get();

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<List<UserDto>>()
                .Which.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task UserController_GetById_ShouldReturnData()
        {
            // Arrange
            _userServiceMock
                .Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_fixture.Create<UserDomain>());

            // Act
            var actual = await _userController.Get("ID");

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<UserDto>()
                .Which.Should().NotBeNull();
        }

        [Fact]
        public async Task UserController_Add_ShouldReturnData()
        {
            var userToAdd = _fixture.Create<UserDto>();

            // Arrange
            _userServiceMock
                .Setup(x => x.AddAsync(It.IsAny<UserDomain>()))
                .ReturnsAsync(_mapper.Map<UserDomain>(userToAdd));

            // Act
            var actual = await _userController.Post(userToAdd);

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<UserDto>()
                .Which.Should().BeEquivalentTo(userToAdd);
        }

        [Fact]
        public async Task UserController_Update_ShouldReturnData()
        {
            var userToAdd = _fixture.Create<UserDto>();

            // Arrange
            _userServiceMock
                .Setup(x => x.UpdateAsync(It.IsAny<UserDomain>()))
                .ReturnsAsync(_mapper.Map<UserDomain>(userToAdd));

            // Act
            var actual = await _userController.Put(userToAdd);

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<UserDto>()
                .Which.Should().BeEquivalentTo(userToAdd);
        }
        [Fact]

        public async Task UserController_Delete_ShouldReturnData()
        {
            const string userToDelete = "ID";

            // Arrange
            _userServiceMock
                .Setup(x => x.DeleteAsync(userToDelete))
                .ReturnsAsync(true);

            // Act
            var actual = await _userController.Delete(userToDelete);

            // Assert
            actual
                .Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<bool>()
                .Which.Should().Be(true);
        }
    }
}
