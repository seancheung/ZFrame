using UnityEngine;
using ZFrame.Debugger;

namespace ZFrame
{
	/// <summary>
	/// This is the game entry class
	/// </summary>
	public class GameEntry : MonoBehaviour
	{

		void Start()
		{
			//var go = new GameObject("MONO", typeof (DelegateMono));
			//go.GetComponent<DelegateMono>().StartEvent+= Show;
			//go.GetComponent<DelegateMono>().StartEvent+= () => ZDebug.Log("Start!");
		}

		void Show()
		{
			ZDebug.Log("Start!");
		}
		//void OnGUI()
		//{
		//	if (!ZSocketServer.Instance.IsListening)
		//	{
		//		if (GUILayout.Button("Start Server"))
		//		{
		//			ZSocketServer.Instance.StartListen();
		//			ZSocketServer.Instance.receiveHandler += ReceiveHandler;
		//		}
		//	}

		//	if (!ZSocketClient.Instance.IsConnected)
		//	{
		//		if (GUILayout.Button("Connect"))
		//		{
		//			ZSocketClient.Instance.msgHandle += MsgHandle;
		//			ZSocketClient.Instance.Setup("127.0.0.1", 6666);
		//			ZSocketClient.Instance.ConnectAsync();
		//		}
		//	}
		//	else
		//	{
		//		if (GUILayout.Button("Send"))
		//		{
		//			ZSocketClient.Instance.Send(Encoding.Default.GetBytes("Hello"));
		//		}
		//	}

		//	GUILayout.Label(_msg);
		//}

		//private void ReceiveHandler()
		//{
		//	_msg += "Client Connected!\r\n";
		//}

		//private void MsgHandle(byte[] data)
		//{
		//	_msg += Encoding.Default.GetString(data) + "\r\n";
		//}
	}
}