using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ControlWSR.Speech
{
	public class Win32
	{
		[DllImport("User32.Dll")]
		public static extern long SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		public static extern bool GetCursorPos(out POINT lpPoint);

		[DllImport("User32.Dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}
	}
}
