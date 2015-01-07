using UnityEngine;
using ZFrame.Debugger;

namespace ZFrame
{
	/// <summary>
	/// This is the game entry class
	/// </summary>
	public class GameEntry : MonoBehaviour
	{
		private string _msg = "";

		private void Start()
		{
			Time.timeScale = 0.5f;
			GameEngine.Instance.Init();
			//AdvancedTimer.Instance.Init();
			//AdvancedTimer.Instance.AddReminder("Test", ReminderType.EveryMinute, DateTime.Now.AddSeconds(5),
			//	() => Debug.Log("Triggered at " + DateTime.Now));
			//ZSocketServer.Instance.StartListen();
			//InvokeTimer.Instance.StartTimer();
			//SimpleTimer.Instance.Init();
			//CoroutineTimer.Instance.Init();
			//NGUIDebug.Log("i love rock'n'roll");
			//ZDebug.Log("i love rock'n'roll");
			//NGUIDebug.Log("this","really","sucks");
			//ZDebug.Log("Hey");

			//ZDebug.LogGUI("This is a test");
			ZDebug.PrintWarnings = true;
			ZDebug.PrintLogs = true;
			ZDebug.LogError("Shit");
			ZDebug.LogWarning("Oh");
			ZDebug.Log("Bullshit");
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