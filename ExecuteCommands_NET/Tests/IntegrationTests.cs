using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace ExecuteCommands_NET.Tests
{
    public class IntegrationTests
    {
        private const string AppExe = "..\\bin\\Release\\net9.0-windows\\ExecuteCommands.exe";
        private const string LogPath = "..\\bin\\Release\\net9.0-windows\\app.log";

        private string RunApp(string args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = AppExe,
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = Process.Start(startInfo);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private string GetLastLogLine()
        {
            var lines = File.ReadAllLines(LogPath);
            return lines.Length > 0 ? lines[^1] : "";
        }

        [Fact]
        public void Test_OpenDownloads()
        {
            RunApp("natural \"open downloads\"");
            string log = GetLastLogLine();
            Assert.Contains("Opened folder: Downloads", log);
        }

        [Fact]
        public void Test_MoveWindowToOtherMonitor()
        {
            RunApp("natural \"move this window to my other monitor\"");
            string log = GetLastLogLine();
            Assert.Contains("Window moved to next monitor", log);
        }

        [Fact]
        public void Test_LaunchEdge()
        {
            RunApp("natural \"open edge\"");
            string log = GetLastLogLine();
            Assert.Contains("Launched app: msedge.exe", log);
        }

        [Fact]
        public void Test_CloseTab()
        {
            RunApp("natural \"close tab\"");
            string log = GetLastLogLine();
            Assert.Contains("Sent Ctrl+W", log);
        }

        [Fact]
        public void Test_FallbackUnknown()
        {
            RunApp("natural \"clothed\"");
            string log = GetLastLogLine();
            Assert.Contains("No matching action", log);
        }
    }
}
