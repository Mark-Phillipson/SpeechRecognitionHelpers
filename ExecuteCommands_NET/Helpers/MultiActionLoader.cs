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
                if (!File.Exists(ConfigPath)) return;
                var json = File.ReadAllText(ConfigPath);
                var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind != JsonValueKind.Array) return;
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
                            Commands[name] = r;
                        }
                    }
                    catch { }
                }
            }
            catch { }
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
