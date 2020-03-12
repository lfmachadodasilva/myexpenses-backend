using FluentAssertions;
using MyExpenses.Models;
using Xunit;

namespace MyExpenses.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var model = new LabelModel { Name = "bla"};
            model.Name.Should().Be("bla");
        }
    }
}
