using System;

namespace ExecuteCommands
{
    // Action type for Visual Studio command execution
    public abstract record ActionBase;
    public record MoveWindowAction(string Target, string Monitor, string? Position, int? WidthPercent, int? HeightPercent) : ActionBase;
    public record CloseTabAction : ActionBase { }
    public record SetWindowAlwaysOnTopAction(string? Application) : ActionBase;
    public record ExecuteVSCommandAction(string CommandName, string? Arguments = null) : ActionBase;
    public record EmojiAction(string? Name, string EmojiText) : ActionBase;
    public record OpenFolderAction(string KnownFolder) : ActionBase;
    public record ShowHelpAction : ActionBase;
    public record LaunchAppAction(string AppExe) : ActionBase;
    public record SendKeysAction(string KeysText) : ActionBase;
}
