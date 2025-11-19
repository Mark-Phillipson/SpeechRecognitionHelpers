using System;
using ExecuteCommands;


namespace ExecuteCommands
{
    /// <summary>
    /// Handles natural language interpretation and action execution.
    /// </summary>
    public class NaturalLanguageInterpreter
    {
        /// <summary>
        /// Interprets the input text and returns an ActionBase (or null if no match)
        /// </summary>
        public ActionBase? InterpretAsync(string text)
        {
            text = text.ToLowerInvariant();

            // Window management
            if (text.Contains("move") && text.Contains("window") && text.Contains("other screen"))
            {
                return new MoveWindowAction(
                    "active", // Target
                    "next",   // Monitor
                    null,      // Position
                    null,      // WidthPercent
                    null       // HeightPercent
                );
            }
            if (text.Contains("window") && (text.Contains("full screen") || text.Contains("maximize")))
            {
                return new MoveWindowAction(
                    "active",
                    "current",
                    "center",
                    100,
                    100
                );
            }
            if (text.Contains("window") && text.Contains("left") && text.Contains("half"))
            {
                return new MoveWindowAction(
                    "active",
                    "current",
                    "left",
                    50,
                    100
                );
            }
            if (text.Contains("window") && text.Contains("right") && text.Contains("half"))
            {
                return new MoveWindowAction(
                    "active",
                    "current",
                    "right",
                    50,
                    100
                );
            }

            // App launch
            if (text.StartsWith("open "))
            {
                var app = text.Substring(5).Trim();
                string exe = app switch
                {
                    "edge" => "msedge.exe",
                    "microsoft edge" => "msedge.exe",
                    "chrome" => "chrome.exe",
                    "visual studio" => "devenv.exe",
                    "visual studio code" => "code.exe",
                    "code" => "code.exe",
                    "outlook" => "outlook.exe",
                    _ => app
                };
                return new LaunchAppAction(exe);
            }

            // Send keys
            if (text.StartsWith("press "))
            {
                var keys = text.Substring(6).Trim();
                return new SendKeysAction(keys);
            }

            // Open folder
            if (text == "open downloads")
            {
                return new OpenFolderAction("Downloads");
            }
            if (text == "open documents")
            {
                return new OpenFolderAction("Documents");
            }

            // Fallback: no match
            return null;
        }

        /// <summary>
        /// Executes the given action. For now, just returns a string describing the action.
        /// </summary>
        public string ExecuteActionAsync(ActionBase action)
        {
            try
            {
                switch (action)
                {
                    case MoveWindowAction move:
                        // TODO: Implement window management using Win32 APIs
                        return $"[Stub] Would move window: Target={move.Target}, Monitor={move.Monitor}, Position={move.Position}, Width={move.WidthPercent}, Height={move.HeightPercent}";

                    case LaunchAppAction launch:
                        try
                        {
                            var psi = new System.Diagnostics.ProcessStartInfo(launch.AppIdOrPath)
                            {
                                UseShellExecute = true
                            };
                            System.Diagnostics.Process.Start(psi);
                            return $"Launched app: {launch.AppIdOrPath}";
                        }
                        catch (Exception ex)
                        {
                            return $"Failed to launch app: {launch.AppIdOrPath}. Error: {ex.Message}";
                        }

                    case SendKeysAction keys:
                        // TODO: Implement key simulation using InputSimulator
                        return $"[Stub] Would send keys: {keys.KeysText}";

                    case OpenFolderAction folder:
                        try
                        {
                            string path = folder.KnownFolder.ToLower() switch
                            {
                                "downloads" => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                                "documents" => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                                _ => Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
                            };
                            var psi = new System.Diagnostics.ProcessStartInfo("explorer.exe", path)
                            {
                                UseShellExecute = true
                            };
                            System.Diagnostics.Process.Start(psi);
                            return $"Opened folder: {folder.KnownFolder} ({path})";
                        }
                        catch (Exception ex)
                        {
                            return $"Failed to open folder: {folder.KnownFolder}. Error: {ex.Message}";
                        }

                    default:
                        return "Unknown action type.";
                }
            }
            catch (Exception ex)
            {
                return $"Error executing action: {ex.Message}";
            }
        }

        public string HandleNaturalAsync(string text)
        {
            var action = InterpretAsync(text);
            if (action == null)
            {
                return $"[Natural mode] No matching action for: {text}";
            }
            var result = ExecuteActionAsync(action);
            return $"[Natural mode] {result}";
        }
    }
}
