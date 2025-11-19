    /// <summary>
    /// Action for showing help/command list.
    /// </summary>
using System;
using ExecuteCommands;

namespace ExecuteCommands
{
    /// <summary>
    /// Handles natural language interpretation and action execution.
    /// </summary>
    public class NaturalLanguageInterpreter
    {
        // P/Invoke for window management
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>
        /// Interprets the input text and returns an ActionBase (or null if no match)
        /// </summary>
        public ActionBase? InterpretAsync(string text)
        {
            string t = text.ToLowerInvariant();
            // Help/introspection
            if (t.Contains("what can i say") || t.Contains("help") || t.Contains("commands") || t.Contains("what are the commands"))
                return new ShowHelpAction();

            // Window management - flexible matching
            if ((t.Contains("move") || t.Contains("put") || t.Contains("snap")) && t.Contains("window") && (t.Contains("right") || t.Contains("to the right") || t.Contains("on the right")))
                return new MoveWindowAction("active", "current", "right", 50, 100);

            if ((t.Contains("move") || t.Contains("put") || t.Contains("snap")) && t.Contains("window") && (t.Contains("left") || t.Contains("to the left") || t.Contains("on the left")))
                return new MoveWindowAction("active", "current", "left", 50, 100);

            if (t.Contains("window") && (t.Contains("full screen") || t.Contains("maximize") || t.Contains("center")))
                return new MoveWindowAction("active", "current", "center", 100, 100);

            if (t.Contains("move") && t.Contains("window") && t.Contains("other screen"))
                return new MoveWindowAction("active", "next", null, null, null);

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
                        {
                            var hWnd = Commands.GetForegroundWindow();
                            if (hWnd == IntPtr.Zero)
                                return "No active window found.";

                            if (move.Position == "center" && move.WidthPercent == 100 && move.HeightPercent == 100)
                            {
                                Commands.ShowCursor(true);
                                ShowWindow(hWnd, 3);
                                return "Window maximized.";
                            }

                            if ((move.Position == "left" || move.Position == "right") && move.WidthPercent == 50 && move.HeightPercent == 100)
                            {
                                var primaryScreen = System.Windows.Forms.Screen.PrimaryScreen;
                                if (primaryScreen == null)
                                    return "No primary screen detected.";
                                var screen = primaryScreen.WorkingArea;
                                int width = screen.Width / 2;
                                int height = screen.Height;
                                int x = move.Position == "left" ? screen.Left : screen.Left + width;
                                int y = screen.Top;
                                bool success = SetWindowPos(hWnd, IntPtr.Zero, x, y, width, height, 0x0040);
                                if (!success)
                                {
                                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                                    System.IO.File.AppendAllText("app.log", $"Failed to move window. Win32 error: {error}\n");
                                    return $"Failed to move window. Win32 error: {error}";
                                }
                                return $"Window moved to {move.Position} half.";
                            }

                            return $"[Stub] Window move not implemented for: {move}";
                        }
                    case OpenFolderAction folder:
                        {
                            string path;
                            switch (folder.KnownFolder.ToLower())
                            {
                                case "downloads":
                                    path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                                    break;
                                case "documents":
                                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                    break;
                                default:
                                    path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                                    break;
                            }
                            var psi = new System.Diagnostics.ProcessStartInfo("explorer.exe", path)
                            {
                                UseShellExecute = true
                            };
                            System.Diagnostics.Process.Start(psi);
                            return $"Opened folder: {folder.KnownFolder} ({path})";
                        }
                    case ShowHelpAction:
                        {
                            return "You may solemnly say:\n" +
                                "- Move this window to the left/right\n" +
                                "- Maximize this window\n" +
                                "- Open downloads/documents\n" +
                                "- Move window to other screen\n" +
                                "- (More natural commands can be added)";
                        }
                    default:
                        return "Unknown action type.";
                }
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("app.log", $"Error executing action: {ex.Message}\n");
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
