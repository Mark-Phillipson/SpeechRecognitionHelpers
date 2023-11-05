﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteCommands
{
    internal static class WinCursors
    {
        [DllImport("user32.dll")]
        private static extern int ShowCursor(bool bShow);


        internal static void ShowCursor()
        {
            ShowCursor(false);
            ShowCursor(true);
            while (ShowCursor(true) < 0)
            {
                ShowCursor(true);
            }
            System.Windows.Forms.Cursor.Show();
        }

        internal static void HideCursor()
        {
            while (ShowCursor(false) >= 0)
            {
                ShowCursor(false);
            }
        }
    }
}
