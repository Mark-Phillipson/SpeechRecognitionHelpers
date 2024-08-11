using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteCommands
{
	public class HoldDownKey
	{


		// Import the SendInput function from user32.dll
		[DllImport("user32.dll")]
		private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

		// Define the INPUT structure for keyboard input
		[StructLayout(LayoutKind.Sequential)]
		struct INPUT
		{
			public uint type;
			public KEYBDINPUT ki;
		}

		// Define the KEYBDINPUT structure
		[StructLayout(LayoutKind.Sequential)]
		struct KEYBDINPUT
		{
			public ushort wVk; // Virtual key code (e.g., 'A' key)
			public ushort wScan;
			public uint dwFlags; // KEYEVENTF_KEYDOWN or KEYEVENTF_KEYUP
			public uint time;
			public IntPtr dwExtraInfo;
		}

		const uint KEYEVENTF_KEYDOWN = 0x0000;
		const uint KEYEVENTF_KEYUP = 0x0002;

		public static void HoldDownSomeKey(System.Windows.Forms.Keys keyToHoldDown, int holdDurationMilliseconds = 2000)
		{
			// Simulate holding down the '?' key
			SimulateKeyPress(keyToHoldDown, holdDurationMilliseconds: holdDurationMilliseconds);

			Console.WriteLine($"Key held for {holdDurationMilliseconds} seconds. Press any key to exit.");
		}

		static void SimulateKeyPress(Keys key, int holdDurationMilliseconds)
		{
			// Create an INPUT structure for key down
			var inputKeyDown = new INPUT
			{
				type = 1, // INPUT_KEYBOARD
				ki = new KEYBDINPUT
				{
					wVk = (ushort)key,
					dwFlags = KEYEVENTF_KEYDOWN,
				}
			};

			// Create an INPUT structure for key up
			var inputKeyUp = new INPUT
			{
				type = 1, // INPUT_KEYBOARD
				ki = new KEYBDINPUT
				{
					wVk = (ushort)key,
					dwFlags = KEYEVENTF_KEYUP,
				}
			};

			// Send the key down event
			SendInput(1, new[] { inputKeyDown }, Marshal.SizeOf(typeof(INPUT)));

			// Wait for the specified duration
			System.Threading.Thread.Sleep(holdDurationMilliseconds);

			// Send the key up event
			SendInput(1, new[] { inputKeyUp }, Marshal.SizeOf(typeof(INPUT)));
		}
	}

}
