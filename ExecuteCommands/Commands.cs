using System.Linq;

namespace ExecuteCommands
{
    public class Commands
    {
        readonly IHandleProcesses _handleProcesses;
        public Commands(IHandleProcesses handleProcesses)
        {
            _handleProcesses = handleProcesses;
        }
        public string PerformCommand(string[] args)
        {
            string[] arguments;

            if (args.Count() < 2)
            {
                arguments = new string[] { args[0], "explorer" };
            }
            else
            {
                arguments = args;
                arguments[1] = arguments[1].Replace("/", "");
                arguments[1] = arguments[1].Trim();
            }
            if (arguments[1] == "explorer" || arguments[1] == "excel" || arguments[1] == "winword" || arguments[1] == "msaccess")
            {
                _handleProcesses.CloseAllProcesses(arguments[1]);
                return $"Closed all Processes of {arguments[1]}";
            }
            else
            {
                return "The arguments supplied does not support any commands in the system";
            }
        }
    }
}
