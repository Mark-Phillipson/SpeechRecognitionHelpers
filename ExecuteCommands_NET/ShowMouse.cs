using System.Runtime.InteropServices;

namespace ExecuteCommands {
	public static class ShowMouse {

		[DllImport("user32.dll")]
		private static extern int ShowCursor(bool bShow);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetCursorPos(int X, int Y);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCursorPos(out POINT lpPoint);

		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT {
			public int X;
			public int Y;
		}

		public static void ShowCursor() {
			while (ShowCursor(true) < 0) {
				ShowCursor(true);
			}
		}

		public static void HideCursor() {
			while (ShowCursor(false) >= 0) {
				ShowCursor(false);
			}
		}

		public static void MakeCursorVisible() {
			HideCursor();
			POINT cursorPosition;
			GetCursorPos(out cursorPosition);
			IntPtr handle = new IntPtr(0);
			SetWindowPos(handle, new IntPtr(-1), cursorPosition.X, cursorPosition.Y, 0, 0, 0x0001 | 0x0040);
		}

	}
}
