using System;
using System.Collections.Generic;

namespace ExecuteCommands
{
    public static class CommandDefinitions
    {
        public static readonly List<(string Command, string Description)> AvailableCommands = new()
        {
            ("maximize window", "Maximize the active window"),
            ("move window to left half", "Move the active window to the left half of the screen"),
            ("move window to right half", "Move the active window to the right half of the screen"),
            ("move window to other monitor", "Move the active window to the next monitor"),
            ("open downloads", "Open the Downloads folder"),
            ("open documents", "Open the Documents folder"),
            ("close tab", "Close the current tab in supported applications"),
            ("send keys", "Send a key sequence to the active window"),
            ("launch app", "Launch a specified application"),
            ("focus app", "Focus a specified application window"),
            ("show help", "Show help and available commands"),
            ("emoji set <name> <emoji>", "Set an emoji for a named shortcut (e.g. emoji set happy 😀)"),
            ("emoji <name>", "Insert the configured emoji for the given name"),
            ("emoji <emoji>", "Insert the given emoji immediately")
        };
        public static readonly List<(string Command, string Description)> VisualStudioCommands = new()
        {
            ("build the solution", "Build the entire solution"),
            ("build the project", "Build the current project"),
            ("start debugging", "Start debugging the startup project"),
            ("start application", "Start without debugging"),
            ("stop debugging", "Stop debugging"),
            ("close tab", "Close the current document tab"),
            ("format document", "Format the current document"),
            ("find in files", "Open the Find in Files dialog"),
            ("go to definition", "Go to definition of symbol"),
            ("rename symbol", "Rename the selected symbol"),
            ("show solution explorer", "Focus Solution Explorer"),
            ("open recent files", "Show recent files"),
        };
        public static readonly List<(string Command, string Description)> VSCodeCommands = new()
        {
            ("open file", "Open a file"),
            ("open folder", "Open a folder"),
            ("close tab", "Close the current tab"),
            ("format document", "Format the current document"),
            ("find in files", "Find in files"),
            ("go to definition", "Go to definition of symbol"),
            ("rename symbol", "Rename the selected symbol"),
            ("show explorer", "Show Explorer"),
            ("show source control", "Show Source Control"),
            ("show extensions", "Show Extensions"),
            ("start debugging", "Start debugging"),
            ("stop debugging", "Stop debugging"),
        };
        public static readonly Dictionary<string, string> VisualStudioCanonicalMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            { "build the solution", "Build.BuildSolution" },
            { "build solution", "Build.BuildSolution" },
            { "build the project", "Build.BuildProject" },
            { "build project", "Build.BuildProject" },
            { "clean solution", "Build.CleanSolution" },
            { "clean the solution", "Build.CleanSolution" },
            { "start debugging", "Debug.Start" },
            { "start application", "Debug.StartWithoutDebugging" },
            { "stop debugging", "Debug.StopDebugging" },
            { "close tab", "Window.CloseDocumentWindow" },
            { "format document", "Edit.FormatDocument" },
            { "find in files", "Edit.FindinFiles" },
            { "go to definition", "Edit.GoToDefinition" },
            { "rename symbol", "Refactor.Rename" },
            { "show solution explorer", "View.SolutionExplorer" },
            { "open recent files", "File.RecentFiles" }
        };
        public static readonly Dictionary<string, ActionBase> PopularCommands = new(StringComparer.OrdinalIgnoreCase)
        {
            { "debug application", new ExecuteVSCommandAction("Debug.Start") },
            { "run application", new ExecuteVSCommandAction("Debug.StartWithoutDebugging") },
            { "stop application", new ExecuteVSCommandAction("Debug.StopDebugging") }
        };
    }
}
