using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Versioning;
using System.Text.Json;

namespace ExecuteCommands.Helpers
{
    [SupportedOSPlatform("windows")]
    public static class VisualStudioHelper
    {
        private const string ProgId = "VisualStudio.DTE";

        private static string GetAppLogPath()
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log");
                return Path.GetFullPath(logPath);
            }
            catch
            {
                return "app.log";
            }
        }

        private static void LogToAppLog(string message)
        {
            try
            {
                File.AppendAllText(GetAppLogPath(), message + Environment.NewLine);
            }
            catch { }
        }

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
                object? dte = GetActiveDTE();
                if (dte == null)
                {
                    Console.WriteLine($"[VS] Could not find running Visual Studio DTE instance.");
                    LogToAppLog($"[VS] Could not find running Visual Studio DTE instance.");
                    return false;
                }
                try
                {
                    // Try to bring Visual Studio main window to the foreground before executing.
                    try
                    {
                        int hwnd = (int)((dynamic)dte).MainWindow.HWnd;
                        if (hwnd != 0)
                        {
                            SetForegroundWindow(new IntPtr(hwnd));
                        }
                    }
                    catch { }

                    ((dynamic)dte).ExecuteCommand(commandName, args);
                }
                catch (Exception ex)
                {
                    // Log and rethrow to outer catch so caller knows execution failed
                    Console.WriteLine($"[VS] ExecuteCommand inner error: {ex.Message}");
                    LogToAppLog($"[VS] ExecuteCommand inner error: {ex.Message} -- {ex}");
                    throw;
                }
                Console.WriteLine($"[VS] Executed: {commandName} {args}");
                LogToAppLog($"[VS] Executed: {commandName} {args}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VS] Error executing '{commandName}': {ex.Message}");
                LogToAppLog($"[VS] Error executing '{commandName}': {ex.Message} -- {ex}");
                return false;
            }
        }

        public static void ExportCommands(string outputPath)
        {
            try
            {
                object? dte = GetActiveDTE();
                if (dte == null)
                {
                    Console.WriteLine($"[VS] Could not find running Visual Studio DTE instance.");
                    LogToAppLog($"[VS] Could not find running Visual Studio DTE instance (ExportCommands).");
                    return;
                }

                var commandList = new List<object>();
                dynamic dteDynamic = dte;

                Console.WriteLine("[VS] Enumerating commands...");
                LogToAppLog("[VS] Enumerating commands...");
                foreach (dynamic cmd in dteDynamic.Commands)
                {
                    try
                    {
                        string name = cmd.Name;
                        if (string.IsNullOrEmpty(name)) continue;

                        object[]? bindings = cmd.Bindings as object[];

                        commandList.Add(new { Name = name, Bindings = bindings ?? Array.Empty<object>() });
                    }
                    catch
                    {
                        // Some commands might throw when accessed
                    }
                }

                string json = JsonSerializer.Serialize(commandList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(outputPath, json);
                Console.WriteLine($"[VS] Exported {commandList.Count} commands to {outputPath}");
                LogToAppLog($"[VS] Exported {commandList.Count} commands to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VS] Error exporting commands: {ex.Message}");
                LogToAppLog($"[VS] Error exporting commands: {ex.Message} -- {ex}");
            }
        }

        /// <summary>
        /// Try to obtain a running Visual Studio DTE object. First attempt common progids via
        /// Marshal.GetActiveObject, then enumerate the Running Object Table (ROT) and pick
        /// the first entry that looks like a VisualStudio.DTE instance.
        /// </summary>
        private static object? GetActiveDTE()
        {
            // Common Visual Studio DTE ProgIDs (try most specific first)
            var progIds = new[] { "VisualStudio.DTE.17.0", "VisualStudio.DTE.16.0", "VisualStudio.DTE.15.0", "VisualStudio.DTE" };
            foreach (var pid in progIds)
            {
                try
                {
                    // Try obtaining via CLSIDFromProgIDEx + GetActiveObject
                    Guid clsid;
                    CLSIDFromProgIDEx(pid, out clsid);
                    GetActiveObject(ref clsid, IntPtr.Zero, out object obj);
                    if (obj != null) return obj;
                }
                catch { }
            }

            // Fallback: enumerate the Running Object Table (ROT)
            try
            {
                int hr = GetRunningObjectTable(0, out IRunningObjectTable rot);
                if (hr != 0 || rot == null)
                    return null;

                rot.EnumRunning(out IEnumMoniker enumMoniker);
                enumMoniker.Reset();
                IMoniker[] monikers = new IMoniker[1];
                IntPtr fetched = IntPtr.Zero;
                // Create bind ctx for display names
                CreateBindCtx(0, out IBindCtx bindCtx);

                while (enumMoniker.Next(1, monikers, fetched) == 0)
                {
                    try
                    {
                        monikers[0].GetDisplayName(bindCtx, null, out string displayName);
                        if (!string.IsNullOrEmpty(displayName) && displayName.IndexOf("VisualStudio.DTE", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            rot.GetObject(monikers[0], out object comObject);
                            if (comObject != null)
                                return comObject;
                        }
                    }
                    catch { }
                }
            }
            catch { }

            return null;
        }

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(uint reserved, out IRunningObjectTable pprot);

        [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Checks whether a Visual Studio command is available in the running DTE instance.
        /// </summary>
        public static bool IsCommandAvailable(string commandName)
        {
            try
            {
                object? dte = GetActiveDTE();
                if (dte == null) return false;
                dynamic d = dte;
                try
                {
                    var cmd = d.Commands.Item(commandName);
                    if (cmd == null) return false;
                    // Some commands expose IsAvailable
                    try { return cmd.IsAvailable; } catch { return true; }
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        [DllImport("ole32.dll", PreserveSig = false)]
        private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

        [DllImport("oleaut32.dll", PreserveSig = false)]
        private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out object ppunk);
    }
}
