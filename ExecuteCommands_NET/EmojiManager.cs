using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ExecuteCommands
{
    public static class EmojiManager
    {
        private static readonly Dictionary<string, string> CommandEmojis = new(StringComparer.OrdinalIgnoreCase)
        {
            { "open downloads", "📥" },
            { "open documents", "🗂️" },
            { "maximize window", "🖥️" },
            { "move window to left half", "⬅️" },
            { "move window to right half", "➡️" },
            { "move window to other monitor", "🔁" },
            { "close tab", "❌" },
            { "send keys", "⌨️" },
            { "launch app", "🚀" },
            { "focus app", "👀" },
            { "show help", "❓" },
            { "happy", "😀" },
            { "sad", "😢" },
            { "thumbs up", "👍" },
            { "heart", "❤️" }
        };
        private static readonly string EmojiMappingsPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "emoji_mappings.json"));
        static EmojiManager() => LoadEmojiMappings();
        private static void LoadEmojiMappings()
        {
            try
            {
                if (File.Exists(EmojiMappingsPath))
                {
                    var json = File.ReadAllText(EmojiMappingsPath);
                    var map = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (map != null)
                    {
                        foreach (var kv in map)
                            CommandEmojis[kv.Key] = kv.Value;
                    }
                }
            }
            catch { }
        }
        private static void SaveEmojiMappings()
        {
            try
            {
                var json = JsonSerializer.Serialize(CommandEmojis);
                File.WriteAllText(EmojiMappingsPath, json);
            }
            catch { }
        }
        public static void SetCommandEmoji(string command, string? emoji)
        {
            if (string.IsNullOrWhiteSpace(command)) return;
            if (string.IsNullOrWhiteSpace(emoji))
            {
                if (CommandEmojis.ContainsKey(command))
                    CommandEmojis.Remove(command);
                SaveEmojiMappings();
            }
            else
            {
                CommandEmojis[command.Trim()] = emoji.Trim();
                SaveEmojiMappings();
            }
        }
        public static string? GetCommandEmoji(string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return null;
            return CommandEmojis.TryGetValue(command.Trim(), out var emoji) ? emoji : null;
        }
        public static IEnumerable<(string Name, string Emoji)> GetAllEmojiMappings()
            => CommandEmojis.Select(kv => (kv.Key, kv.Value)).ToArray();
    }
}
