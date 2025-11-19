namespace ExecuteCommands
{
    public record ShowHelpAction() : ActionBase;
    // Base type for all actions
    public abstract record ActionBase;

    public record MoveWindowAction(
        string Target, // e.g. "active"
        string Monitor, // "current" | "next" | "previous"
        string? Position, // "left" | "right" | "top" | "bottom" | "center" | null
        int? WidthPercent,
        int? HeightPercent
    ) : ActionBase;

    public record LaunchAppAction(
        string AppIdOrPath // e.g. "msedge.exe", "code.exe", or full path
    ) : ActionBase;

    public record SendKeysAction(
        string KeysText // e.g. "control shift b"
    ) : ActionBase;

    public record OpenFolderAction(
        string KnownFolder // e.g. "Downloads", "Documents"
    ) : ActionBase;

    public record SetWindowAlwaysOnTopAction(
        string? Application // e.g. "code", "msedge", or null for current
    ) : ActionBase;
}
