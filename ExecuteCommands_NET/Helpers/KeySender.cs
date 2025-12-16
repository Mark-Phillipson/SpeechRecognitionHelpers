namespace ExecuteCommands.Helpers
{
    // Handles sending key sequences and typing text
    public class KeySender
    {
        // TODO: Move key sending methods here from NaturalLanguageInterpreter

        public static string SendKeys(ExecuteCommands.SendKeysAction keys)
        {
            string logPath = "app.log";
            string keysText = keys.KeysText?.Trim().ToLowerInvariant() ?? string.Empty;
            System.IO.File.AppendAllText(logPath, $"[DEBUG] KeySender.SendKeys called with: '{keysText}'\n");

            if (keysText == "ctrl alt tab" || keysText == "control alt tab")
            {
                // Use keybd_event to send Ctrl+Alt+Tab
                System.IO.File.AppendAllText(logPath, "[DEBUG] Sending Ctrl+Alt+Tab key sequence.\n");
                // Ctrl down
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyDown(0x11); // VK_CONTROL
                // Alt down
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyDown(0x12); // VK_MENU (Alt)
                // Tab down
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyDown(0x09); // VK_TAB
                // Tab up
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyUp(0x09);
                // Alt up
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyUp(0x12);
                // Ctrl up
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyUp(0x11);
                System.IO.File.AppendAllText(logPath, "[DEBUG] Sent Ctrl+Alt+Tab.\n");
                return "[KeySender.SendKeys] Sent Ctrl+Alt+Tab.";
            }
            else if (keysText == "control ," || keysText == "ctrl ,")
            {
                // Send Ctrl+Comma
                System.IO.File.AppendAllText(logPath, "[DEBUG] Sending Ctrl+Comma key sequence.\n");
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyDown(0x11); // VK_CONTROL
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyDown(0xBC); // VK_OEM_COMMA
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyUp(0xBC);
                ExecuteCommands.Helpers.WindowFocusHelper.SendKeyUp(0x11);
                System.IO.File.AppendAllText(logPath, "[DEBUG] Sent Ctrl+Comma.\n");
                return "[KeySender.SendKeys] Sent Ctrl+Comma.";
            }
            else
            {
                System.IO.File.AppendAllText(logPath, $"[ERROR] Unsupported key sequence: '{keysText}'\n");
                return $"[KeySender.SendKeys] Unsupported key sequence: '{keysText}'";
            }
        }

        // Sends a shortcut string like "ctrl+alt+t" or "win+h" or "ctrl+shift+f5".
        // Supported modifiers: ctrl/control, alt, shift, win/lwin
        // Supported keys: single letters, digits, function keys (f1..f24), and common punctuation via VK codes
        public static string SendShortcut(string shortcut)
        {
            if (string.IsNullOrWhiteSpace(shortcut)) return "";
            var parts = shortcut.Split(new[] { '+', '-' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim().ToLowerInvariant()).ToArray();
            var modifiers = new List<byte>();
            byte? mainKey = null;

            foreach (var p in parts)
            {
                if (p == "ctrl" || p == "control") modifiers.Add(0x11); // VK_CONTROL
                else if (p == "alt" || p == "menu") modifiers.Add(0x12); // VK_MENU
                else if (p == "shift") modifiers.Add(0x10); // VK_SHIFT
                else if (p == "win" || p == "lwin" || p == "rwin") modifiers.Add(0x5B); // VK_LWIN (use left win)
                else if (p.Length == 1 && char.IsLetterOrDigit(p[0])) mainKey = (byte)char.ToUpperInvariant(p[0]);
                else if (p.StartsWith("f") && int.TryParse(p.Substring(1), out var fn) && fn >= 1 && fn <= 24)
                {
                    mainKey = (byte)(0x70 + fn - 1); // VK_F1 = 0x70
                }
                else
                {
                    // simple punctuation, NumPad keys and other special cases
                    switch (p)
                    {
                        case "enter": mainKey = 0x0D; break;
                        case "esc": case "escape": mainKey = 0x1B; break;
                        case "tab": mainKey = 0x09; break;
                        case "space": mainKey = 0x20; break;
                        case ",": mainKey = 0xBC; break; // VK_OEM_COMMA
                        case ".": mainKey = 0xBE; break; // VK_OEM_PERIOD
                        case "comma": mainKey = 0xBC; break;
                        case "period": case "dot": mainKey = 0xBE; break;
                        case "backspace": mainKey = 0x08; break;
                        // NumPad add/subtract (common Talon toggle keys)
                        case "add": case "numpadadd": case "plus": mainKey = 0x6B; break; // VK_ADD
                        case "subtract": case "numpadsubtract": case "minus": mainKey = 0x6D; break; // VK_SUBTRACT
                        default:
                            // unknown token
                            System.IO.File.AppendAllText("app.log", $"[DEBUG] SendShortcut: unknown token '{p}' in '{shortcut}'\n");
                            break;
                    }
                }
            }

            // Press modifiers
            foreach (var m in modifiers) WindowFocusHelper.SendKeyDown(m);
            if (mainKey.HasValue) WindowFocusHelper.SendKeyDown(mainKey.Value);
            if (mainKey.HasValue) WindowFocusHelper.SendKeyUp(mainKey.Value);
            // Release modifiers in reverse order
            for (int i = modifiers.Count - 1; i >= 0; i--) WindowFocusHelper.SendKeyUp(modifiers[i]);

            return $"[KeySender.SendShortcut] Sent '{shortcut}'";
        }
    }
}
