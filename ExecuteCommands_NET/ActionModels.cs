using System;
using System.Collections.Generic;

namespace ExecuteCommands
{
    // Action type for Visual Studio command execution
    public abstract record ActionBase;
    public record RunMultipleActionsAction(string Name, List<ActionBase> Actions, bool ContinueOnError = true, int DelayMsBetween = 250) : ActionBase;
    public record MoveWindowAction(string Target, string Monitor, string? Position, int? WidthPercent, int? HeightPercent) : ActionBase;
    // Default TimeoutMs = 0 disables auto-submit while allowing auto-start
    // (the form only auto-submits when TimeoutMs > 0).
    public record OpenVoiceDictationFormAction(int TimeoutMs = 0) : ActionBase;
    public record CloseTabAction : ActionBase { }
    public record SetWindowAlwaysOnTopAction(string? Application) : ActionBase;
    public record ExecuteVSCommandAction(string CommandName, string? Arguments = null) : ActionBase;
    public record EmojiAction(string? Name, string EmojiText) : ActionBase;
    public record OpenFolderAction(string KnownFolder) : ActionBase;
    public record FocusWindowAction(string WindowTitleSubstring) : ActionBase;
    public record OpenWebsiteAction(string Url) : ActionBase;
    public record ShowHelpAction : ActionBase;
    public record LaunchAppAction(string AppExe) : ActionBase;
    public record SendKeysAction(string KeysText) : ActionBase;
}
