using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ExecuteCommands.Helpers
{
    public static class WordReplacementHelper
    {
        private static Dictionary<string, string>? _wordReplacements;
        private static Dictionary<string, string> GetWordReplacements()
        {
            if (_wordReplacements != null) return _wordReplacements;
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "word_replacements.json");
            path = Path.GetFullPath(path);
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _wordReplacements = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            else
            {
                _wordReplacements = new Dictionary<string, string>();
            }
            return _wordReplacements;
        }

        public static string ApplyWordReplacements(string text)
        {
            var replacements = GetWordReplacements();
            foreach (var kvp in replacements)
            {
                // Replace whole words only
                text = Regex.Replace(text, $"\\b{Regex.Escape(kvp.Key)}\\b", kvp.Value, RegexOptions.IgnoreCase);
            }
            return text;
        }
    }
}
