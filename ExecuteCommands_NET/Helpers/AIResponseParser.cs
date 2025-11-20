using Newtonsoft.Json;

namespace ExecuteCommands.Helpers
{
    public static class AIResponseParser
    {
        /// <summary>
        /// Parses an AI response, stripping markdown formatting and deserializing JSON.
        /// </summary>
        public static T? ParseAIResponse<T>(string response)
        {
            response = response.Trim();
            if (response.StartsWith("````"))
            {
                int firstNewline = response.IndexOf('\n');
                int lastBacktick = response.LastIndexOf("```", StringComparison.Ordinal);
                if (firstNewline >= 0 && lastBacktick > firstNewline)
                {
                    response = response.Substring(firstNewline + 1, lastBacktick - firstNewline - 1).Trim();
                }
                else
                {
                    response = response.Replace("```json", "").Replace("```", "").Trim();
                }
            }
            // Fallback: handle OpenAI responses with 'action' instead of 'type'
            if (response.Contains("\"action\": \"open_application\""))
            {
                // Try to extract application name
                var match = System.Text.RegularExpressions.Regex.Match(response, "\\\"application\\\"\\s*:\\s*\\\"([^\\\"]+)\\\"");
                var app = match.Success ? match.Groups[1].Value : "calc.exe";
                var fixedJson = $"{{ \"type\": \"LaunchAppAction\", \"AppIdOrPath\": \"{app.ToLower().Replace("calculator", "calc.exe")}\" }}";
                return JsonConvert.DeserializeObject<T>(fixedJson);
            }
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
