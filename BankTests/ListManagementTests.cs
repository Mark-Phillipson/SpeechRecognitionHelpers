using BrowseScripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ListManagementTests
    {
        [TestMethod]
        public void BuildListFilter_ReturnCorrectFilter()
        {
            var commandName = "Test <Test1> <Test2>";
            var filter = "";

            var actual = ListManagement.BuildListFilter(commandName, filter);

            Assert.AreEqual("name = 'Test1' Or name = 'Test2'", actual);
        }
        [TestMethod]
        public void BuildListFilter_ReturnBlankFilter()
        {
            var commandName = "Test";
            var filter = "";
            var expected = "";

            var actual = ListManagement.BuildListFilter(commandName, filter);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BuildListFilter_ReturnCorrectFilterAddTo()
        {
            var commandName = "Test <Test1> <Test2>";
            var filter = "name = 'Test0'";
            var expected = "name = 'Test0' Or name = 'Test1' Or name = 'Test2'";

            var actual = ListManagement.BuildListFilter(commandName, filter);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BuildListFilter_BadData()
        {
            string commandName = null;
            string filter = null;
            var expected = "";

            var actual = ListManagement.BuildListFilter(commandName, filter);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void BuildListFilter_BadDataPart2()
        {
            string commandName = "";
            string filter = null;
            var expected = "";

            var actual = ListManagement.BuildListFilter(commandName, filter);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void HasLists_ReturnTrue()
        {
            string commandName = "Test <Test1>";
            var expected = true;
            var actual = ListManagement.HasLists(commandName);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void HasLists_ReturnFalse()
        {
            string commandName = "Test";
            var expected = false;
            var actual = ListManagement.HasLists(commandName);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void HasLists_BadData()
        {
            string commandName = null;
            var expected = false;
            var actual = ListManagement.HasLists(commandName);

            Assert.AreEqual(expected, actual);
        }
    }
}
