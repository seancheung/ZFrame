using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace ZFrame.Net
{
	public class ZSocketServer : MonoSingleton<ZSocketServer>, IDisposable, IGameDisposable
	{
		public delegate void MsgHandle(byte[] data);

		protected TcpListener listener;
		protected Thread serverThread;
		protected int port = 6666;

		public MsgHandle msgHandle;
		protected bool isListening;

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
		}

		public void StartListen()
		{
			Close();
			listener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
			listener.Start();
			listener.BeginAcceptTcpClient(ReceiveCallback, null);
			isListening = true;
		}

		IEnumerator Listen()
		{
			TcpClient client = listener.AcceptTcpClient();
			NetworkStream stream = client.GetStream();
			if (stream.DataAvailable)
			{
				byte[] data = new byte[256];
				stream.Write(data, 0, data.Length);
				OnReceived(data);
			}
			yield return new WaitForEndOfFrame();
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			Debug.Log("ReceiveCallback");
			TcpClient client = listener.EndAcceptTcpClient(ar);

			NetworkStream stream = client.GetStream();
			if (stream.DataAvailable)
			{
				byte[] data = new byte[256];
				stream.Write(data, 0, data.Length);
				OnReceived(data);
			}

			listener.BeginAcceptTcpClient(ReceiveCallback, null);
		}

		protected virtual void OnReceived(byte[] data)
		{
			//...
			Debug.Log("OnReceived");
		}

		public void Close()
		{
			if (listener != null)
			{
				listener.Stop();
			}
		}

		public bool DisposeOnApplicationQuit()
		{
			(this as IDisposable).Dispose();
			ReleaseInstance();
			return true;
		}

		bool IGameDisposable.Dispose()
		{
			(this as IDisposable).Dispose();
			return true;
		}

		void IDisposable.Dispose()
		{
			if (serverThread != null)
			{
				serverThread.Abort();
			}
		}
	}
}