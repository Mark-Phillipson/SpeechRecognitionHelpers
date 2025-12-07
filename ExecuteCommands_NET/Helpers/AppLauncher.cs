using System;
using System.IO;
namespace ExecuteCommands.Helpers
{
    // Handles application launching and focusing
    public class AppLauncher
    {
        // TODO: Move app launching/focusing methods here from NaturalLanguageInterpreter
        public static string Launch(ExecuteCommands.LaunchAppAction app)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log");
                logPath = Path.GetFullPath(logPath);
                try { System.IO.File.AppendAllText(logPath, $"[DEBUG] AppLauncher.Launch: Requested launch '{app?.AppExe}'\n"); } catch { }

                if (app == null || string.IsNullOrWhiteSpace(app.AppExe))
                {
                    try { System.IO.File.AppendAllText(logPath, "[ERROR] AppLauncher.Launch: No app specified.\n"); } catch { }
                    return "No application specified to launch.";
                }

                var psi = new System.Diagnostics.ProcessStartInfo(app.AppExe)
                {
                    UseShellExecute = true
                };

                try
                {
                    System.Diagnostics.Process.Start(psi);
                    try { System.IO.File.AppendAllText(logPath, $"[INFO] AppLauncher.Launch: Started '{app.AppExe}' successfully.\n"); } catch { }
                    return $"Launched: {app.AppExe}";
                }
                catch (Exception ex)
                {
                    try { System.IO.File.AppendAllText(logPath, $"[ERROR] AppLauncher.Launch: Failed to start '{app.AppExe}': {ex.Message}\n"); } catch { }
                    // Try fallback: use explorer to open by verb
                    try
                    {
                        var psi2 = new System.Diagnostics.ProcessStartInfo("explorer.exe", app.AppExe) { UseShellExecute = true };
                        System.Diagnostics.Process.Start(psi2);
                        try { System.IO.File.AppendAllText(logPath, $"[INFO] AppLauncher.Launch: Fallback explorer launched for '{app.AppExe}'.\n"); } catch { }
                        return $"Launched via explorer: {app.AppExe}";
                    }
                    catch (Exception ex2)
                    {
                        try { System.IO.File.AppendAllText(logPath, $"[ERROR] AppLauncher.Launch: Explorer fallback failed for '{app.AppExe}': {ex2.Message}\n"); } catch { }
                        return $"Failed to launch '{app.AppExe}': {ex.Message}; fallback: {ex2.Message}";
                    }
                }
            }
            catch (Exception exOuter)
            {
                try { System.IO.File.AppendAllText(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "bin", "app.log")), $"[ERROR] AppLauncher.Launch: Unexpected: {exOuter.Message}\n"); } catch { }
                return $"App launch failed: {exOuter.Message}";
            }
        }
    }
}
