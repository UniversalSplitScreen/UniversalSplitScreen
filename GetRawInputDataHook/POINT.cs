using System.Runtime.InteropServices;

namespace GetRawInputDataHook
{
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int x;//needs to be int(not long) or XNA games won't work for y-coordinate
		public int y;
	}
}
