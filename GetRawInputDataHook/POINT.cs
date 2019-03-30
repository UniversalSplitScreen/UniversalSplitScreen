using System.Runtime.InteropServices;

namespace GetRawInputDataHook
{
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public long x;
		public long y;
	}
}
