using System;
using Xunit;
using ExecuteCommands;

namespace ExecuteCommands_NET.Tests
{
    public class WebsiteNavigatorUnitTests
    {
        [Theory]
        [InlineData("open upwork dotcom", "https://www.upwork.com")]
        [InlineData("go to upwork.com", "https://www.upwork.com")]
        [InlineData("visit upwork dot com", "https://www.upwork.com")]
        [InlineData("browse upwork", "https://www.upwork.com")]
        [InlineData("open upworkdotcom", "https://www.upwork.com")]
        public void Test_UpworkRecognition(string input, string expectedUrl)
        {
            Assert.True(WebsiteNavigator.TryParseWebsiteCommand(input, out var url));
            Assert.Equal(expectedUrl, url);
        }
    }
}
