using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExecuteCommands
{
    public static class WebsiteNavigator
    {
        private static readonly Dictionary<string, string> WebsiteMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            { "youtube", "https://www.youtube.com" },
            { "upwork", "https://www.upwork.com" },
            { "reddit", "https://www.reddit.com" },
            { "github", "https://github.com" },
            { "gmail", "https://mail.google.com" },
            { "google", "https://www.google.com" },
            { "facebook", "https://www.facebook.com" },
            { "twitter", "https://twitter.com" },
            { "linkedin", "https://www.linkedin.com" },
            { "amazon", "https://www.amazon.com" },
            { "stackoverflow", "https://stackoverflow.com" },
            { "bing", "https://www.bing.com" },
            { "yahoo", "https://www.yahoo.com" },
            { "netflix", "https://www.netflix.com" },
            { "bbc", "https://www.bbc.com" },
            { "twitch", "https://www.twitch.tv" },
            { "discord", "https://discord.com" },
            { "office", "https://www.office.com" },
            { "onenote", "https://www.onenote.com" },
            { "outlook", "https://outlook.live.com" },
            { "azure", "https://portal.azure.com" }
        };

        // Include 'launch' as a trigger since users often say 'launch <site>'
        private static readonly string[] WebsiteTriggers = new[] { "open ", "go to ", "browse ", "visit ", "launch " };

        public static bool TryParseWebsiteCommand(string text, out string? url)
        {
            url = null;
            foreach (var trigger in WebsiteTriggers)
            {
                if (text.StartsWith(trigger, StringComparison.OrdinalIgnoreCase))
                {
                    var siteName = text.Substring(trigger.Length).Trim().ToLowerInvariant();
                    // Remove common suffixes and variants
                    siteName = siteName.Replace("dotcom", "").Replace("dot com", "").Replace("dot org", "").Replace("dot net", "").Replace("dot co.uk", "");
                    siteName = siteName.Replace(".com", "").Replace(".org", "").Replace(".net", "").Replace(".co.uk", "");
                    siteName = siteName.Replace("the ", "").Replace("website", "").Replace("site", "");
                    siteName = siteName.Replace(" ", "").Trim();

                    // Try direct match
                    if (WebsiteMappings.TryGetValue(siteName, out var directUrl) && directUrl != null)
                    {
                        url = directUrl;
                        return true;
                    }
                    // Try with 'com' appended
                    if (WebsiteMappings.TryGetValue(siteName + "com", out var comUrl) && comUrl != null)
                    {
                        url = comUrl;
                        return true;
                    }
                    // Try with 'dotcom' appended
                    if (WebsiteMappings.TryGetValue(siteName + "dotcom", out var dotcomUrl) && dotcomUrl != null)
                    {
                        url = dotcomUrl;
                        return true;
                    }
                    // Fallback: check for popular sites
                    foreach (var key in WebsiteMappings.Keys)
                    {
                        if (siteName.Contains(key.ToLowerInvariant()))
                        {
                            url = WebsiteMappings[key];
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static string LaunchWebsite(string url)
        {
            try
            {
                var psi = new ProcessStartInfo("msedge.exe", url)
                {
                    UseShellExecute = true
                };
                Process.Start(psi);
                return $"Launched Edge with URL: {url}";
            }
            catch (Exception ex)
            {
                return $"Failed to launch Edge with URL: {url}. Error: {ex.Message}";
            }
        }
    }
}
