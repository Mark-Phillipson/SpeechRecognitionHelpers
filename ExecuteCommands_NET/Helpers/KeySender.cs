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
    }
}
