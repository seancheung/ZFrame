using System;
using System.Net;
using System.Net.Sockets;
using ZFrame.MonoBase;

namespace ZFrame.Net
{
	public class ZSocketServer : MonoSingleton<ZSocketServer>
	{
		public delegate void ReceiveHandler();

		protected TcpListener listener;
		protected int port = 6666;

		public ReceiveHandler receiveHandler;
		public bool IsListening { get; protected set; }

		public void StartListen()
		{
			Close();
			listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
			listener.Start();
			listener.BeginAcceptTcpClient(ReceiveCallback, null);
			IsListening = true;
		}

		//private IEnumerator Listen()
		//{
		//	TcpClient client = listener.AcceptTcpClient();
		//	NetworkStream stream = client.GetStream();
		//	if (stream.DataAvailable)
		//	{
		//		byte[] data = new byte[256];
		//		stream.Write(data, 0, data.Length);
		//		OnReceived(data);
		//	}
		//	yield return new WaitForEndOfFrame();
		//}

		private void ReceiveCallback(IAsyncResult ar)
		{
			//TcpClient client = listener.EndAcceptTcpClient(ar);

			//if (receiveHandler != null)
			//{
			//	receiveHandler();
			//}
			//NetworkStream stream = client.GetStream();
			//if (stream.DataAvailable)
			//{
			//	byte[] data = new byte[256];
			//	stream.Write(data, 0, data.Length);
			//	OnReceived(data);
			//}
			//listener.BeginAcceptTcpClient(ReceiveCallback, null);
		}

		//protected virtual void OnReceived(byte[] data)
		//{
		//	//...
		//	Debug.Log("OnReceived");
		//}

		public void Close()
		{
			if (IsListening)
			{
				listener.Stop();
				IsListening = false;
			}
		}

		private void OnDestroy()
		{
			Close();
		}
	}
}