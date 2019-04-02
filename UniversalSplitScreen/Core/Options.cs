namespace UniversalSplitScreen.Core
{
	public class Options
	{
		public bool SendRawMouseInput { get; set; } = true;
		public bool SendRawKeyboardInput { get; set; } = false;//TODO: Implement
		public bool SendNormalMouseInput { get; set; } = true;
		public bool SendNormalKeyboardInput { get; set; } = true;

		public bool SendWM_ACTIVATE { get; set; } = true;
		public bool SendWM_SETFOCUS { get; set; } = false;

		public bool RefreshWindowBoundsOnMouseClick { get; set; } = false;

		public bool DrawMouse { get; set; } = true;
		public int DrawMouseEveryXMilliseconds { get; set; } = 15;

		public bool Hook_FilterRawInput { get; set; } = false;
		public bool Hook_FilterWindowsMouseInput { get; set; } = false;
		public bool Hook_GetForegroundWindow { get; set; } = false;

		public ushort EndVKey { get; set; } = 0x23;
	}
}
