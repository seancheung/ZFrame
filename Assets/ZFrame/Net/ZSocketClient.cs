using System;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;

namespace ZFrame.Net
{
	public class ZSocketClient : MonoSingleton<ZSocketClient>, IDisposable, IGameDisposable
	{
		public delegate void MsgHandle(byte[] data);

		protected Socket socket;
		protected string ip;
		protected int port;
		protected byte[] receiveBuffer;
		protected int connectTryOut = 3;
		private int _connectTry;
		protected Queue rcvQueue = Queue.Synchronized(new Queue());
		protected Queue sendQueue = Queue.Synchronized(new Queue());
		protected static object locker = new object();
		public MsgHandle msgHandle;

		private void Start()
		{
			GameEngine.Instance.RegisterDispose(this);
		}

		public virtual void Setup(string ip, int port)
		{
			Setup(ip, port, 8142, 5000);
		}

		public virtual void Setup(string ip, int port, int bufferSize, int receiveTimeOut)
		{
			this.ip = ip;
			this.port = port;
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			receiveBuffer = new byte[bufferSize];
			socket.ReceiveTimeout = receiveTimeOut;
		}

		public bool IsConnected
		{
			get { return socket != null && socket.Connected; }
		}

		public bool Connect()
		{
			if (!IsConnected)
			{
				socket.Connect(ip, port);
				socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, null);
				return true;
			}
			return false;
		}

		public void ConnectAsync()
		{
			if (!IsConnected)
			{
				socket.BeginConnect(ip, port, ConnectCallback, null);
			}
		}

		private void ConnectCallback(IAsyncResult ar)
		{
			socket.EndConnect(ar);
			if (IsConnected)
			{
				socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, null);
			}
			else if (_connectTry < connectTryOut)
			{
				_connectTry++;
				socket.BeginConnect(ip, port, ConnectCallback, null);
			}
		}

		protected void ReceiveCallback(IAsyncResult ar)
		{
			int received = socket.EndReceive(ar);
			if (received <= 0)
				return;
			byte[] data = new byte[received];
			Buffer.BlockCopy(receiveBuffer, 0, data, 0, received);

			//data is what you received
			OnReceived(data);

			//Start
			socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, null);
		}

		protected virtual void OnReceived(byte[] data)
		{
			lock (locker)
			{
				rcvQueue.Enqueue(data);
			}
		}

		protected virtual bool SendAsync(byte[] data)
		{
			if (data == null)
			{
				return false;
			}
			SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
			socketAsyncEventArgs.SetBuffer(data, 0, data.Length);
			return socket.SendAsync(socketAsyncEventArgs);
		}

		public virtual bool Send(byte[] data)
		{
			lock (locker)
			{
				sendQueue.Enqueue(data);
			}
			return true;
		}

		protected virtual void Update()
		{
			HandleReceive();
			HandleSend();
		}

		protected virtual void HandleReceive()
		{
			if (IsConnected)
			{
				lock (locker)
				{
					if (rcvQueue.Count > 0)
					{
						byte[] data = rcvQueue.Dequeue() as byte[];
						if (msgHandle != null)
						{
							msgHandle(data);
						}
					}
				}
			}
		}

		protected virtual void HandleSend()
		{
			if (IsConnected)
			{
				lock (locker)
				{
					if (sendQueue.Count > 0)
					{
						byte[] data = sendQueue.Dequeue() as byte[];
						SendAsync(data);
					}
				}
			}
		}

		public void Close()
		{
			if (IsConnected)
			{
				socket.Close();
			}
		}

		public bool DisposeOnApplicationQuit()
		{
			(this as IDisposable).Dispose();
			return true;
		}

		bool IGameDisposable.Dispose()
		{
			(this as IDisposable).Dispose();
			ReleaseInstance();
			return true;
		}

		void IDisposable.Dispose()
		{
			Close();
		}
	}
}