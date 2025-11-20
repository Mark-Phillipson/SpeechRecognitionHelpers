using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace ExecuteCommands.Helpers
{
    [SupportedOSPlatform("windows")]
    public static class VisualStudioHelper
    {
        private const string ProgId = "VisualStudio.DTE";

        /// <summary>
        /// Attempts to execute a Visual Studio command via COM Automation (EnvDTE).
        /// </summary>
        /// <param name="commandName">The command canonical name (e.g., "Build.BuildSolution")</param>
        /// <param name="args">Optional arguments</param>
        /// <returns>True if successful, False if VS not found or error.</returns>
        public static bool ExecuteCommand(string commandName, string args = "")
        {
            try
            {
                object? dte = GetActiveObject(ProgId);
                if (dte == null)
                {
                    Console.WriteLine($"[VS] Could not find running instance of {ProgId}.");
                    return false;
                }
                ((dynamic)dte).ExecuteCommand(commandName, args);
                Console.WriteLine($"[VS] Executed: {commandName} {args}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VS] Error executing '{commandName}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// .NET Core replacement for Marshal.GetActiveObject
        /// </summary>
        private static object? GetActiveObject(string progId)
        {
            try
            {
                Guid clsid;
                CLSIDFromProgIDEx(progId, out clsid);
                object? obj;
                GetActiveObject(ref clsid, IntPtr.Zero, out obj);
                return obj;
            }
            catch
            {
                return null;
            }
        }

        [DllImport("ole32.dll", PreserveSig = false)]
        private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

        [DllImport("oleaut32.dll", PreserveSig = false)]
        private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out object ppunk);
    }
}
