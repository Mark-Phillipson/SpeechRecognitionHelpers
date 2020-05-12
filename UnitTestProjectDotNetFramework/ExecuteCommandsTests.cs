using ExecuteCommands;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
    public class ExecuteCommandsTests
    {
        //https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
        public static IEnumerable<object[]> Data()
        {
            string[] arguments1 = new string[] { "test", "/ explorer" };
            string[] arguments2 = new string[] { "test", "/ winword" };
            string[] arguments3 = new string[] { "test", "/ excel" };
            string[] arguments4 = new string[] { "test", "/ msaccess" };
            string[] arguments5 = new string[] { "test", "/ test" };
            string[] arguments6 = new string[] { "test" };
            string[] arguments7 = new string[] { null };
            var output = new List<object[]>
             {
                 new object[] { arguments1, "Closed all Processes of explorer" },
                 new object[] { arguments2, "Closed all Processes of winword" },
                 new object[] { arguments3, "Closed all Processes of excel" },
                 new object[] { arguments4, "Closed all Processes of msaccess" },
                 new object[] { arguments5, "The arguments supplied does not support any commands in the system" },
                 new object[] { arguments6, "Closed all Processes of explorer" },
                 new object[] { arguments7, "Closed all Processes of explorer" },
             };
            return output;
        }
        [Theory]
        [MemberData(nameof(Data))]
        public void PerformCommandGetExpectedResult(string[] arguments, string expected)
        {
            var mock = new Mock<IHandleProcesses>();
            Commands commands = new Commands(mock.Object);
            var actual = commands.PerformCommand(arguments);

            Assert.Equal(expected, actual);
        }
    }
}
