using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ExecuteCommands
{
    /// <summary>
    /// Loads optional word replacements from a JSON file and provides an Apply method
    /// to deterministically replace whole words in input text (case-insensitive).
    /// The JSON file should be a simple object mapping strings to strings, e.g.
    /// { "closed": "close", "ferries": "fairies" }
    /// </summary>
    internal static class WordReplacementLoader
    {
        private static readonly Dictionary<string, string> _replacements = new(StringComparer.OrdinalIgnoreCase);

        public static IReadOnlyDictionary<string, string> Replacements => _replacements;

        public static void Load()
        {
            try
            {
                var candidates = new[] {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "word_replacements.json"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "word_replacements.json"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "word_replacements.json")
                };
                foreach (var cand in candidates)
                {
                    var p = Path.GetFullPath(cand);
                    if (File.Exists(p))
                    {
                        var json = File.ReadAllText(p);
                        try
                        {
                            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                            if (dict != null)
                            {
                                _replacements.Clear();
                                foreach (var kv in dict)
                                {
                                    if (string.IsNullOrWhiteSpace(kv.Key)) continue;
                                    _replacements[kv.Key.ToLowerInvariant()] = kv.Value ?? string.Empty;
                                }
                                Log($"Loaded word replacements from: {p}");
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log($"Failed to parse word replacements file {p}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"LoadWordReplacements exception: {ex.Message}");
            }
        }

        public static string Apply(string text)
        {
            if (string.IsNullOrEmpty(text) || _replacements.Count == 0)
                return text;

            string original = text;
            foreach (var kv in _replacements)
            {
                try
                {
                    text = Regex.Replace(text, "\\b" + Regex.Escape(kv.Key) + "\\b", kv.Value, RegexOptions.IgnoreCase);
                }
                catch { }
            }
            if (!string.Equals(original, text, StringComparison.Ordinal))
            {
                Log($"Applied word replacements: '{original}' -> '{text}'");
            }
            return text;
        }

        private static void Log(string msg)
        {
            try
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log");
                var full = Path.GetFullPath(logPath);
                File.AppendAllText(full, $"[WordReplacementLoader] {msg}\n");
            }
            catch { }
        }
    }
}
