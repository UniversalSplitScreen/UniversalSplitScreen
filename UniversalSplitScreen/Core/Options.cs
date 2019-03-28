namespace UniversalSplitScreen.Core
{
	public class Options
	{
		public static bool SendRawMouseInput { get; set; } = true;
		public static bool SendRawKeyboardInput { get; set; } = false;//TODO: Implement
		public static bool SendNormalMouseInput { get; set; } = true;
		public static bool SendNormalKeyboardInput { get; set; } = true;

		public static bool SendWM_ACTIVATE { get; set; } = true;
		public static bool SendWM_SETFOCUS { get; set; } = false;

		public static bool RefreshWindowBoundsOnMouseClick { get; set; } = false;

		public static bool DrawMouse { get; set; } = true;
		public static int DrawMouseEveryXMilliseconds { get; set; } = 15;
	}
}
