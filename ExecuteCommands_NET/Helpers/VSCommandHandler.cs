namespace ExecuteCommands.Helpers
{
    // Handles Visual Studio and VS Code command execution
    public class VSCommandHandler
    {
        // TODO: Move VS/VSCode command methods here from NaturalLanguageInterpreter

        public static string ExecuteVSCommand(ExecuteCommands.ExecuteVSCommandAction vsCmd)
        {
            // Call VisualStudioHelper to execute the command
            try
            {
                bool success = VisualStudioHelper.ExecuteCommand(vsCmd.CommandName, vsCmd.Arguments ?? "");
                if (success)
                {
                    return $"[VSCommandHandler.ExecuteVSCommand] Executed: {vsCmd.CommandName}";
                }
                else
                {
                    return $"[VSCommandHandler.ExecuteVSCommand] Command failed or not supported: {vsCmd.CommandName}";
                }
            }
            catch (Exception ex)
            {
                return $"[VSCommandHandler.ExecuteVSCommand] Exception: {ex.Message}";
            }
        }
    }
}
