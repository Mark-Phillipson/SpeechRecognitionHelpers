using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ExecuteCommands.Helpers
{
    public static class VisualStudioShortcutHelper
    {
        // Finds the latest .vssettings file in the user's VS settings folder
        public static string? FindLatestVsSettingsFile()
        {
            string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string settingsRoot = Path.Combine(userProfile, "AppData", "Local", "Microsoft", "VisualStudio");
            if (!Directory.Exists(settingsRoot)) return null;
            var vsDirs = Directory.GetDirectories(settingsRoot)
                .Where(d => d.Contains("17.0") || d.Contains("16.0") || d.Contains("15.0"))
                .OrderByDescending(d => d);
            foreach (var dir in vsDirs)
            {
                var files = Directory.GetFiles(dir, "*.vssettings");
                if (files.Length > 0)
                {
                    // Return the most recent file
                    return files.OrderByDescending(f => File.GetLastWriteTime(f)).First();
                }
            }
            return null;
        }

        // Parses the .vssettings file and returns a dictionary of command -> shortcut
        public static Dictionary<string, string> ParseShortcuts(string vsSettingsPath)
        {
            var shortcuts = new Dictionary<string, string>();
            if (!File.Exists(vsSettingsPath)) return shortcuts;
            var doc = XDocument.Load(vsSettingsPath);
            var keyBindings = doc.Descendants("KeyBindings").FirstOrDefault();
            if (keyBindings == null) return shortcuts;
            foreach (var kb in keyBindings.Descendants("Shortcut"))
            {
                var command = kb.Attribute("Command")?.Value;
                var keys = kb.Attribute("Keys")?.Value;
                if (!string.IsNullOrEmpty(command) && !string.IsNullOrEmpty(keys))
                {
                    shortcuts[command] = keys;
                }
            }
            return shortcuts;
        }

        // Gets the shortcut for a specific command (e.g., View.SolutionExplorer)
        public static string? GetShortcutForCommand(string command)
        {
            var settingsFile = FindLatestVsSettingsFile();
            if (settingsFile == null) return null;
            var shortcuts = ParseShortcuts(settingsFile);
            if (shortcuts.TryGetValue(command, out var keys))
                return keys;
            return null;
        }
    }
}
