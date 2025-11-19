using Newtonsoft.Json;

namespace ExecuteCommands.Helpers
{
    public static class AIResponseParser
    {
        /// <summary>
        /// Parses an AI response, stripping markdown formatting and deserializing JSON.
        /// </summary>
        public static T ParseAIResponse<T>(string response)
        {
            response = response.Trim();
            if (response.StartsWith("```"))
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
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}
