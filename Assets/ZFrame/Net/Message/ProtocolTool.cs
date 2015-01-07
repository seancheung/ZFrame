using System;
using System.Runtime.InteropServices;

namespace ZFrame.Net.Message
{
	public static class ProtocolTool
	{
		public static T ToStruct<T>(byte[] arr) where T : struct
		{
			int size = Marshal.SizeOf(typeof (T));
			IntPtr ptr = Marshal.AllocHGlobal(size);
			Marshal.Copy(arr, 0, ptr, size);
			object obj = Marshal.PtrToStructure(ptr, typeof (T));
			Marshal.FreeHGlobal(ptr);
			return (T) obj;
		}

		public static byte[] ToBytes<T>(T structure) where T : struct
		{
			IntPtr handle = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
			Marshal.StructureToPtr(structure, handle, true);
			//
			return null;
		}
	}
}