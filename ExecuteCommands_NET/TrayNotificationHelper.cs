using System;
using System.Drawing;
using System.Windows.Forms;

namespace ExecuteCommands
{
    public static class TrayNotificationHelper
    {
        private static NotifyIcon? _notifyIcon;
        private static Icon? _appIcon;

        public static void ShowNotification(string title, string message, int timeout = 5000)
        {
            if (_notifyIcon == null)
            {
                _notifyIcon = new NotifyIcon();
                // Use a default system icon if no app icon is available
                _appIcon = SystemIcons.Information;
                _notifyIcon.Icon = _appIcon;
                _notifyIcon.Visible = true;
                _notifyIcon.Text = "ExecuteCommands.NET";
            }
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = message;
            _notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            _notifyIcon.ShowBalloonTip(timeout);
        }

        public static void Dispose()
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
        }
    }
}
