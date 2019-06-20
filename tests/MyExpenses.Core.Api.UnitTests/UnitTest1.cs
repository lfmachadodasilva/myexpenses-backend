using MyExpenses.Core.Api.Controllers;
using Xunit;

namespace MyExpenses.Core.Api.UnitTests
{
    public class UnitTest1
    {
        public readonly ValuesController _valuesController;

        public UnitTest1()
        {
            _valuesController = new ValuesController();
        }

        [Fact]
        public void Test1()
        {
            Assert.NotNull(_valuesController);
        }
    }
}
