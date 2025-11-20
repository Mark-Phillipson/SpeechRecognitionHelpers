using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ExecuteCommands.Helpers
{
        public static class VisualStudioShortcutHelper
        {
            // Cached shortcuts dictionary
            private static Dictionary<string, string> _cachedShortcuts = new Dictionary<string, string>();

            // Refreshes the cached shortcuts from the latest .vssettings file
            public static void RefreshShortcuts()
            {
                var settingsFile = FindLatestVsSettingsFile();
                if (settingsFile != null)
                {
                    _cachedShortcuts = ParseShortcuts(settingsFile);
                }
            }

            // Gets the cached shortcuts dictionary
            public static Dictionary<string, string> GetCachedShortcuts()
            {
                if (_cachedShortcuts.Count == 0)
                {
                    RefreshShortcuts();
                }
                return _cachedShortcuts;
            }

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
                        // Try to find official KeyBinding format first
                        var keyBindingsCategory = doc.Descendants("Category").FirstOrDefault(e => (string?)e.Attribute("name") == "Environment_KeyBindings");
                        if (keyBindingsCategory != null)
                        {
                            var keyBindings = keyBindingsCategory.Descendants("KeyBinding");
                            foreach (var kb in keyBindings)
                            {
                                var command = kb.Attribute("id")?.Value;
                                var keys = kb.Attribute("key1")?.Value;
                                if (!string.IsNullOrEmpty(command) && !string.IsNullOrEmpty(keys))
                                {
                                    shortcuts[command] = keys;
                                }
                            }
                        }
                        // Fallback to custom Shortcut format if present
                        var customKeyBindings = doc.Descendants("KeyBindings").FirstOrDefault();
                        if (customKeyBindings != null)
                        {
                            foreach (var kb in customKeyBindings.Descendants("Shortcut"))
                            {
                                var command = kb.Attribute("Command")?.Value;
                                var keys = kb.Attribute("Keys")?.Value;
                                if (!string.IsNullOrEmpty(command) && !string.IsNullOrEmpty(keys))
                                {
                                    shortcuts[command] = keys;
                                }
                            }
                        }
                        return shortcuts;
                    }

                    // Gets the shortcut for a specific command (e.g., View.SolutionExplorer)
                    public static string? GetShortcutForCommand(string command)
                    {
                        var shortcuts = GetCachedShortcuts();
                        if (shortcuts.TryGetValue(command, out var keys))
                            return keys;
                        return null;
                    }
                }
                }
