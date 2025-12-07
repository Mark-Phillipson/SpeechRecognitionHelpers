using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ExecuteCommands.Helpers
{
    public static class MultiActionLoader
    {
        public static Dictionary<string, ExecuteCommands.RunMultipleActionsAction> Commands { get; } = new(StringComparer.OrdinalIgnoreCase);

        private static readonly string ConfigPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "multi_actions.json"));

        public static void Load()
        {
            try
            {
                Commands.Clear();
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");
                try { File.AppendAllText(logPath, $"[DEBUG] MultiActionLoader.ConfigPath: {ConfigPath}\n"); } catch { }
                if (!File.Exists(ConfigPath))
                {
                    try { File.AppendAllText(logPath, $"[WARN] MultiActionLoader: config not found at {ConfigPath}\n"); } catch { }
                    return;
                }
                var json = File.ReadAllText(ConfigPath);
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind != JsonValueKind.Array) return;
                var registered = new List<string>();
                foreach (var item in doc.RootElement.EnumerateArray())
                {
                    try
                    {
                        var name = item.GetProperty("Name").GetString() ?? string.Empty;
                        bool continueOnError = true;
                        int delay = 250;
                        if (item.TryGetProperty("ContinueOnError", out var pco)) continueOnError = pco.GetBoolean();
                        if (item.TryGetProperty("DelayMsBetween", out var pd)) delay = pd.GetInt32();

                        var actions = new List<ExecuteCommands.ActionBase>();
                        if (item.TryGetProperty("Actions", out var aProp) && aProp.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var a in aProp.EnumerateArray())
                            {
                                var act = DeserializeAction(a);
                                if (act != null) actions.Add(act);
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(name) && actions.Count > 0)
                        {
                            var r = new ExecuteCommands.RunMultipleActionsAction(name, actions, continueOnError, delay);
                            // store the entry under its original name
                            Commands[name] = r;
                            registered.Add(name);
                            // also store a normalized/compact key to allow flexible matching (e.g. "setup" vs "set up")
                            try
                            {
                                var normalized = NormalizeKey(name);
                                if (!string.IsNullOrWhiteSpace(normalized) && !Commands.ContainsKey(normalized))
                                {
                                    Commands[normalized] = r;
                                    registered.Add(normalized);
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }
                }
            }
            catch { }
            try
            {
                var logPath2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");
                // write keys that were registered (if any)
                if (Commands.Count > 0)
                {
                    try { File.AppendAllText(logPath2, $"[DEBUG] MultiActionLoader loaded keys: {string.Join(", ", Commands.Keys)}\n"); } catch { }
                }
            }
            catch { }
        }

        public static string NormalizeKey(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return string.Empty;
            try
            {
                var low = s.ToLowerInvariant();
                // remove punctuation (keep word chars and whitespace)
                low = System.Text.RegularExpressions.Regex.Replace(low, "[^\\w\\s]", "");
                // collapse whitespace
                low = System.Text.RegularExpressions.Regex.Replace(low, "\\s+", " ").Trim();
                // compact (remove spaces) so variants like "setup" and "set up" match
                var compact = low.Replace(" ", "");
                return compact;
            }
            catch
            {
                return s.ToLowerInvariant();
            }
        }

        private static ExecuteCommands.ActionBase? DeserializeAction(JsonElement elem)
        {
            if (elem.ValueKind != JsonValueKind.Object) return null;
            string type = elem.GetProperty("Type").GetString() ?? string.Empty;
            try
            {
                switch (type)
                {
                    case "SendKeysAction":
                        return new ExecuteCommands.SendKeysAction(elem.GetProperty("KeysText").GetString() ?? string.Empty);
                    case "LaunchAppAction":
                        return new ExecuteCommands.LaunchAppAction(elem.GetProperty("AppExe").GetString() ?? string.Empty);
                    case "OpenWebsiteAction":
                        return new ExecuteCommands.OpenWebsiteAction(elem.GetProperty("Url").GetString() ?? string.Empty);
                    case "OpenFolderAction":
                        return new ExecuteCommands.OpenFolderAction(elem.GetProperty("KnownFolder").GetString() ?? string.Empty);
                    case "FocusWindowAction":
                        return new ExecuteCommands.FocusWindowAction(elem.GetProperty("WindowTitleSubstring").GetString() ?? string.Empty);
                    case "ExecuteVSCommandAction":
                        return new ExecuteCommands.ExecuteVSCommandAction(elem.GetProperty("CommandName").GetString() ?? string.Empty);
                    case "MoveWindowAction":
                        var target = elem.GetProperty("Target").GetString() ?? "active";
                        var monitor = elem.GetProperty("Monitor").GetString() ?? "current";
                        string? pos = null;
                        int? w = null; int? h = null;
                        if (elem.TryGetProperty("Position", out var pp) && pp.ValueKind == JsonValueKind.String) pos = pp.GetString();
                        if (elem.TryGetProperty("WidthPercent", out var wp) && wp.ValueKind == JsonValueKind.Number) w = wp.GetInt32();
                        if (elem.TryGetProperty("HeightPercent", out var hp) && hp.ValueKind == JsonValueKind.Number) h = hp.GetInt32();
                        return new ExecuteCommands.MoveWindowAction(target, monitor, pos, w, h);
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
