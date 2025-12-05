using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;

namespace ExecuteCommands.Helpers
{
    public static class AccessibilityHelper
    {
        public static bool TryFindVisualStudioElement(string accessibleName, out AutomationElement? element)
        {
            element = null;
            var process = Process.GetProcessesByName("devenv").FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero);
            if (process == null)
                return false;

            try
            {
                var root = AutomationElement.FromHandle(process.MainWindowHandle);
                if (root == null)
                    return false;

                var condition = new PropertyCondition(AutomationElement.NameProperty, accessibleName);
                element = root.FindFirst(TreeScope.Descendants, condition);
                return element != null;
            }
            catch (ElementNotAvailableException)
            {
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
