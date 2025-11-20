using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace ExecuteCommands_NET.Tests
{
    public class IntegrationTests
    {
        // Use absolute path to main executable and log file
        private static readonly string SolutionRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));
        private static readonly string AppExe = Path.Combine(SolutionRoot, "bin", "Debug", "net10.0-windows", "ExecuteCommands.exe");
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log");

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
            // [Fact]
            // public void Test_OpenDownloads()
            // {
            //     RunApp("natural \"open downloads\"");
            //     Assert.True(LogContains("Opened folder: Downloads"), "Expected log to contain 'Opened folder: Downloads'");
            // }

            // [Fact]
            // public void Test_MoveWindowToOtherMonitor()
            // {
            //     RunApp("natural \"move this window to my other monitor\"");
            //     Assert.True(LogContains("Window moved to next monitor"), "Expected log to contain 'Window moved to next monitor'");
            // }

            // [Fact]
            // public void Test_LaunchEdge()
            // {
            //     RunApp("natural \"open edge\"");
            //     Assert.True(LogContains("Launched app: msedge.exe"), "Expected log to contain 'Launched app: msedge.exe'");
            // }

            // [Fact]
            // public void Test_CloseTab()
            // {
            //     RunApp("natural \"close tab\"");
            //     Assert.True(LogContains("Sent Ctrl+W"), "Expected log to contain 'Sent Ctrl+W'");
            // }

            // [Fact]
            // public void Test_FallbackUnknown()
            // {
            //     RunApp("natural \"foobar unknown command\"");
            //     Assert.True(LogContains("No matching action"), "Expected log to contain 'No matching action'");
            // }

            // [Fact]
            // public void Test_RestoreWindow()
            // {
            //     RunApp("natural \"restore window\"");
            //     Assert.True(LogContains("Window maximized"), "Expected log to contain 'Window maximized'");
            // }
        [Fact]
        public void Test_CloseTab()
        {
            RunApp("natural \"close tab\"");
            Assert.True(LogContains("Sent Ctrl+W"), "Expected log to contain 'Sent Ctrl+W'");
        }

        [Fact]
        public void Test_FallbackUnknown()
        {
            RunApp("natural \"clothed\"");
            Assert.True(LogContains("No matching action"), "Expected log to contain 'No matching action'");
        }


        // [Fact]
        // public void Test_PutWindowOnTop()
        // {
        //     RunApp("natural \"put this window on top\"");
        //     Assert.True(LogContains("Window set to always on top"), "Expected log to contain 'Window set to always on top'");
        // }

        // [Fact]
        // public void Test_FloatWindowAboveOthers()
        // {
        //     RunApp("natural \"float this window above other windows\"");
        //     Assert.True(LogContains("Window set to always on top"), "Expected log to contain 'Window set to always on top'");
        // }

        [Fact]
        public void Test_RestoreWindow()
        {
            RunApp("natural \"restore this window\"");
            Assert.True(LogContains("Window maximized"), "Expected log to contain 'Window maximized'");
        }
    }
}
