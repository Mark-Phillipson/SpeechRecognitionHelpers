using DictationBoxMSP;

using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using WindowsInput;
using WindowsInput.Native;

namespace ExecuteCommands
{
    public class Commands
    {
        readonly IHandleProcesses _handleProcesses;
        readonly InputSimulator inputSimulator = new InputSimulator();
        public Commands(IHandleProcesses handleProcesses)
        {
            _handleProcesses = handleProcesses;
        }
        public string PerformCommand(string[] args)
        {
            string[] arguments;
            //MessageBox.Show("line 20");
            if (args.Count() < 2)
            {
                //arguments = new string[] { args[0], "Error Message: There is an error in the program!" };
                //arguments = new string[] { args[0], "explorer" };
                //arguments = new string[] { args[0], "show cursor" };
                //arguments = new string[] { args[0], "sapisvr" };
                //arguments = new string[] { args[0], "click" };
                //arguments = new string[] { args[0], "/startstoplistening" };
                arguments = new string[] { args[0], "ScrollRight" };
                //arguments = new string[] { args[0], "StartContinuousDictation" };

            }
            else
            {
                arguments = args;
                arguments[1] = arguments[1].Replace("/", "");
                arguments[1] = arguments[1].Trim();
            }
            //MessageBox.Show("Got here With argument1 " + arguments[1]);

            if (arguments[1] == "explorer" || arguments[1] == "excel" || arguments[1] == "winword" || arguments[1] == "msaccess" || arguments[1] == "sapisvr")
            {
                _handleProcesses.CloseAllProcesses(arguments[1]);
                return $"Closed all Processes of {arguments[1]}";
            }
            else if (arguments[1].ToLower().Contains("script for databases"))
            {
                var clipboard = Clipboard.GetText();
                ManipulateSelection manipulateSelection = new ManipulateSelection();
                var result = manipulateSelection.CreateSqlScriptForAllDatabases(clipboard);
                Clipboard.SetText(result);
                DisplayMessage displayMessage = new DisplayMessage("The clipboard should now contain the revised SQL script.");
                Application.Run(displayMessage);
                return $"The clipboard should now contain the revised SQL script.";
            }
            else if (arguments[1].ToLower() == "show cursor")
            {
                WinCursors.ShowCursor();
                return "The cursor should now be Visible";
            }
            else if (arguments[1].ToLower() == "startcontinuousdictation")
            {
                string processName = "SpeechContinuousRecognition";
                var process = Process.GetProcessesByName(processName).FirstOrDefault();
                if (process != null)
                {//Not currently working, not sure why!
                    ManipulateWindow manipulateWindow = new ManipulateWindow();
                    manipulateWindow.BringMainWindowToFront(processName);
                }
                else
                {
                    Process.Start("C:\\Users\\MPhil\\source\\repos\\ControlWSR\\SpeechContinuousRecognition\\bin\\Release\\net7.0-windows\\SpeechContinuousRecognition.exe");
                }
                return " Moved over to continuous dictation ";
            }
            else if (arguments[1].ToLower() == "scrollright")
            {
                inputSimulator.Mouse.HorizontalScroll(10);
                return " Scrolled right ";
            }
            else if (arguments[1].ToLower() == "scrollleft")
            {
                inputSimulator.Mouse.HorizontalScroll(-10);
                return " Scrolled left ";
            }
            else if (arguments[1].ToLower().Contains("startstoplistening"))
            {
                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
                inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LWIN);
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
                ////MessageBox.Show("Start / stop listening has fired");
                return "Windows speech recognition listening state has now been toggled";
            }
            else if (arguments[1].StartsWith("Error Message:"))
            {
                DisplayMessage displayMessage = new DisplayMessage(arguments[1]);
                Application.Run(displayMessage);

                return "Message Displayed";
            }
            else if (arguments[1].ToLower() == "click")
            {
                inputSimulator.Mouse.LeftButtonClick();
                return "mouseclick performed";
            }
            else if (arguments[1].StartsWith("Speak Message"))
            {
                var message = arguments[1].Replace("Speak Message", "");
                SpeakMessage.TextToSpeech(message);
                return "Message Spoken";
            }
            else
            {
                return "The arguments supplied does not support any commands in the system";
            }
        }
    }
}

