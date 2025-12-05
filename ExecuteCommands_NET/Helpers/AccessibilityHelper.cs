using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Automation;

namespace ExecuteCommands.Helpers
{
    public static class AccessibilityHelper
    {
        private static readonly ControlType[] ActionableControlTypes = new[]
        {
            ControlType.Button,
            ControlType.MenuItem,
            ControlType.ListItem
        };

        public static bool TryGetVisualStudioRoot(out AutomationElement? root)
        {
            root = null;
            var process = Process.GetProcessesByName("devenv").FirstOrDefault(p => p.MainWindowHandle != IntPtr.Zero);
            if (process == null)
                return false;

            try
            {
                root = AutomationElement.FromHandle(process.MainWindowHandle);
                return root != null;
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

        public static bool TryFindVisualStudioElement(string accessibleName, out AutomationElement? element)
        {
            element = null;
            if (!TryGetVisualStudioRoot(out var root) || root == null)
                return false;

            return TryFindVisualStudioElement(root, accessibleName, out element);
        }

        public static bool TryFindVisualStudioElement(AutomationElement root, string accessibleName, out AutomationElement? element)
        {
            element = null;
            if (root == null)
                return false;

            try
            {
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

        public static IReadOnlyList<AutomationElement> EnumerateActionableControls(AutomationElement root, int maxCount)
        {
            if (root == null || maxCount <= 0)
                return Array.Empty<AutomationElement>();

            try
            {
                var condition = CreateActionableCondition();
                var matches = root.FindAll(TreeScope.Descendants, condition);
                var results = new List<AutomationElement>(Math.Min(matches.Count, maxCount));

                for (int i = 0; i < matches.Count && results.Count < maxCount; i++)
                {
                    var candidate = matches[i];
                    if (string.IsNullOrWhiteSpace(candidate.Current.Name))
                        continue;

                    results.Add(candidate);
                }

                return results;
            }
            catch (ElementNotAvailableException)
            {
                return Array.Empty<AutomationElement>();
            }
            catch
            {
                return Array.Empty<AutomationElement>();
            }
        }

        private static Condition CreateActionableCondition()
        {
            var enabledCondition = new PropertyCondition(AutomationElement.IsEnabledProperty, true);
            var visibleCondition = new PropertyCondition(AutomationElement.IsOffscreenProperty, false);
            var typeConditions = new Condition[ActionableControlTypes.Length];

            for (int i = 0; i < ActionableControlTypes.Length; i++)
            {
                typeConditions[i] = new PropertyCondition(AutomationElement.ControlTypeProperty, ActionableControlTypes[i]);
            }

            var typeCondition = new OrCondition(typeConditions);
            return new AndCondition(enabledCondition, visibleCondition, typeCondition);
        }
    }
}
