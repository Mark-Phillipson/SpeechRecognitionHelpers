using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ExecuteCommands.Helpers
{
    public class VisualStudioCommandInfo
    {
        public string Name { get; set; } = "";
        public List<string> Bindings { get; set; } = new();
    }

    public static class VisualStudioCommandLoader
    {
        private static List<VisualStudioCommandInfo> _commands = new();

        public static void LoadCommands(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    var json = File.ReadAllText(path);
                    _commands = JsonSerializer.Deserialize<List<VisualStudioCommandInfo>>(json) ?? new();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load VS commands: {ex.Message}");
                }
            }
        }

        public static List<VisualStudioCommandInfo> GetCommands() => _commands;

        public static VisualStudioCommandInfo? FindCommand(string searchText)
        {
            if (_commands.Count == 0) return null;

            var searchParts = searchText.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (searchParts.Length == 0) return null;

            var bestMatch = _commands
                .Select(c => new { Command = c, Score = ScoreCommand(c.Name, searchParts) })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            // Threshold: require at least some significant matching
            // If the user types "build", and we match "Build.BuildSolution" (score high)
            // If the user types "foo", and we match nothing (score 0)
            
            // Let's say we need to match at least 50% of the search terms?
            // Or just return the best match if it's decent.
            
            if (bestMatch != null && bestMatch.Score >= searchParts.Length * 2) // Heuristic
            {
                return bestMatch.Command;
            }

            return null;
        }

        private static int ScoreCommand(string commandName, string[] searchParts)
        {
            int score = 0;
            string lowerName = commandName.ToLowerInvariant();
            
            // Penalize very long command names slightly to prefer shorter, more exact matches?
            // Or prefer matches where the parts appear in order?
            
            foreach (var part in searchParts)
            {
                int index = lowerName.IndexOf(part);
                if (index >= 0)
                {
                    score += part.Length * 2; // Base score for matching characters
                    
                    // Bonus for matching start of segments (e.g. "Build" in "Build.Solution")
                    if (index == 0 || lowerName[index - 1] == '.')
                    {
                        score += 5;
                    }
                }
                else
                {
                    score -= 1; // Penalty for missing words
                }
            }
            
            return score;
        }
    }
}
