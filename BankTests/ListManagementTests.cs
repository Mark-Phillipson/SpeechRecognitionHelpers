using BrowseScripts;
using Xunit;


namespace UnitTests
{

    public class ListManagementTests
    {
        [Theory]
        [InlineData("Test <Test1> <Test2>", "", "name = 'Test1' Or name = 'Test2'")]
        [InlineData("Test <Test1> <Test2>", "Test0", "name= 'Test0' Or name = 'Test1' Or name = 'Test2'")]
        [InlineData("Test", "Test0", "name = 'Test0'")]
        [InlineData(null, null, "")]
        [InlineData("", null, "")]
        public void BuildListFilter_ReturnCorrectFilter(string commandName, string filter, string expected)
        {
            var actual = ListManagement.BuildListFilter(commandName, filter);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData("Test <Test1>", true)]
        [InlineData("Test", false)]
        [InlineData(null, false)]
        public void HasLists_ReturnCorrectValue(string commandName, bool expected)
        {
            var actual = ListManagement.HasLists(commandName);
            Assert.Equal(expected, actual);
        }

    }
}
