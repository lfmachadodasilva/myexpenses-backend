using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace lfmachadodasilva.MyExpenses.Api.UnitTests
{
    //public class UnitTest1
    //{
    //    public readonly ValuesController _valuesController;

    //    public UnitTest1()
    //    {
    //        _valuesController = new ValuesController();
    //    }

    //    [Fact]
    //    public void NotNull()
    //    {
    //        Assert.NotNull(_valuesController);
    //    }

    //    [Fact]
    //    public async Task GetAll()
    //    {
    //        // act
    //        var actual = await _valuesController.Get();

    //        // assert
    //        actual
    //            .Should().BeOfType<OkObjectResult>()
    //            .Which.Value.Should().BeEquivalentTo(new string[] { "value1", "value2" });
    //    }

    //    [Fact]
    //    public async Task Get()
    //    {
    //        // act
    //        var actual = await _valuesController.Get(It.IsAny<int>());

    //        // assert
    //        actual
    //            .Should().BeOfType<OkObjectResult>()
    //            .Which.Value.Should().Be("value");
    //    }

    //    [Fact]
    //    public async Task Post()
    //    {
    //        // act
    //        var actual = _valuesController.Post(It.IsAny<string>());

    //        Func<Task> sutMethod = async () => { await actual; };
    //        await sutMethod.Should().NotThrowAsync<Exception>();
    //    }
    //}
}
