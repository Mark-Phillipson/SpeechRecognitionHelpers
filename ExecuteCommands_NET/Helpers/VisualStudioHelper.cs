using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text.Json;
using WindowsInput;
using WindowsInput.Native;

namespace ExecuteCommands.Helpers
{
    [SupportedOSPlatform("windows")]
    public static class VisualStudioHelper
    {
        // Try several common ProgIDs for different Visual Studio versions.
        private static readonly string[] ProgIdsToTry = new[] {
            "VisualStudio.DTE",
            "VisualStudio.DTE.17.0",
            "VisualStudio.DTE.16.0",
            "VisualStudio.DTE.15.0",
            "VisualStudio.DTE.14.0"
        };
        /// <summary>
        /// Attempts to execute a Visual Studio command via COM Automation (EnvDTE).
        /// </summary>
        /// <param name="commandName">The command canonical name (e.g., "Build.BuildSolution")</param>
        /// <param name="args">Optional arguments</param>
        /// <returns>True if successful, False if VS not found or error.</returns>
        public static bool ExecuteCommand(string commandName, string args = "")
        {
            // Try COM automation first
            object? dte = null;
            try
            {
                dte = GetActiveObject(null);
                if (dte != null)
                {
                    try
                    {
                        ((dynamic)dte).ExecuteCommand(commandName, args);
                        Console.WriteLine($"[VS] Executed via DTE: {commandName} {args}");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[VS] DTE ExecuteCommand failed for '{commandName}': {ex.Message}");
                        // fall through to keyboard fallback
                    }
                }
                else
                {
                    Console.WriteLine($"[VS] No DTE instance found - will try keyboard fallback for '{commandName}'.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VS] Exception while obtaining DTE: {ex.Message}. Will try keyboard fallback for '{commandName}'.");
            }

            // Keyboard fallback for common Visual Studio commands
            try
            {
                var sim = new InputSimulator();
                // Map canonical command names to one or more key strokes (modifiers + key)
                var mapping = new Dictionary<string, List<(VirtualKeyCode[] Modifiers, VirtualKeyCode Key)>>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Build.BuildSolution", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, VirtualKeyCode.VK_B) } },
                    { "Build.BuildProject", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, VirtualKeyCode.VK_B) } },
                    { "Debug.Start", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (Array.Empty<VirtualKeyCode>(), VirtualKeyCode.F5) } },
                    { "Debug.StartWithoutDebugging", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL }, VirtualKeyCode.F5) } },
                    { "Debug.StopDebugging", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.SHIFT }, VirtualKeyCode.F5) } },
                    { "Window.CloseDocumentWindow", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL }, VirtualKeyCode.F4) } },
                    { "Edit.FormatDocument", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_K), (new[]{ VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_D) } },
                    { "Edit.FindinFiles", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, VirtualKeyCode.VK_F) } },
                    { "Edit.GoToDefinition", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (Array.Empty<VirtualKeyCode>(), VirtualKeyCode.F12) } },
                    { "Refactor.Rename", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_R), (new[]{ VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_R) } },
                    { "View.SolutionExplorer", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL, VirtualKeyCode.MENU }, VirtualKeyCode.VK_L) } },
                    { "File.RecentFiles", new List<(VirtualKeyCode[] , VirtualKeyCode)>{ (new[]{ VirtualKeyCode.CONTROL }, VirtualKeyCode.VK_R) } }
                };

                if (mapping.TryGetValue(commandName, out var strokes))
                {
                    foreach (var stroke in strokes)
                    {
                        // If no modifiers, send the key directly
                        if (stroke.Modifiers == null || stroke.Modifiers.Length == 0)
                        {
                            sim.Keyboard.KeyPress(stroke.Key);
                        }
                        else
                        {
                            sim.Keyboard.ModifiedKeyStroke(stroke.Modifiers, stroke.Key);
                        }
                        // Small delay between chords
                        System.Threading.Thread.Sleep(80);
                    }
                    Console.WriteLine($"[VS] Executed via keyboard fallback: {commandName}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"[VS] No keyboard fallback mapping for: {commandName}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VS] Keyboard fallback failed for '{commandName}': {ex.Message}");
                return false;
            }
        }

        public static void ExportCommands(string outputPath)
        {
            try
            {
                object? dte = GetActiveObject(null);
                if (dte == null)
                {
                    Console.WriteLine($"[VS] Could not find a running Visual Studio DTE instance.");
                    return;
                }

                var commandList = new List<object>();
                dynamic dteDynamic = dte;

                Console.WriteLine("[VS] Enumerating commands...");
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VS] Error exporting commands: {ex.Message}");
            }
        }

        /// <summary>
        /// .NET Core replacement for Marshal.GetActiveObject
        /// </summary>
        /// <summary>
        /// Attempts to locate a running Visual Studio DTE object. Tries several common ProgIDs
        /// (unversioned and versioned) and returns the first running instance found, or null.
        /// </summary>
        private static object? GetActiveObject(string? progId)
        {
            // If a specific progId was passed, try it first, then fall back to common variants.
            var candidates = new List<string>();
            if (!string.IsNullOrWhiteSpace(progId)) candidates.Add(progId!);
            candidates.AddRange(ProgIdsToTry.Where(p => !string.IsNullOrWhiteSpace(p) && (progId == null || !p.Equals(progId, StringComparison.OrdinalIgnoreCase))));

            foreach (var candidate in candidates)
            {
                try
                {
                    Guid clsid;
                    CLSIDFromProgIDEx(candidate, out clsid);
                    object? obj;
                    GetActiveObject(ref clsid, IntPtr.Zero, out obj);
                    return obj;
                }
                catch
                {
                    // Try next candidate
                }
            }
            return null;
        }

        [DllImport("ole32.dll", PreserveSig = false)]
        private static extern void CLSIDFromProgIDEx([MarshalAs(UnmanagedType.LPWStr)] string progId, out Guid clsid);

        [DllImport("oleaut32.dll", PreserveSig = false)]
        private static extern void GetActiveObject(ref Guid rclsid, IntPtr reserved, [MarshalAs(UnmanagedType.Interface)] out object ppunk);
    }
}
