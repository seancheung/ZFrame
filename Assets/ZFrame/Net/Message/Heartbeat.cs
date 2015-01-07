using System.Runtime.InteropServices;

namespace ZFrame.Net.Message
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public struct Heartbeat
	{
		public int ID;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)] public string Encoded;

		public Heartbeat(int id)
		{
			ID = 0;
			Encoded = "HEARTBEAT";
		}
	}
}