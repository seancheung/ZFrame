
using System.Runtime.InteropServices;

namespace ZFrame.Net.Message
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public struct Message
	{
		public int ID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
		public string Info;
	}

}