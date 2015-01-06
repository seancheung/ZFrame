using System;
using System.Text;
using UnityEngine;
using ZFrame.Net;
using ZFrame.Timer;

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
			
		}

		void OnGUI()
		{
			if (!ZSocketServer.Instance.IsListening)
			{
				if (GUILayout.Button("Start Server"))
				{
					ZSocketServer.Instance.StartListen();
					ZSocketServer.Instance.receiveHandler += ReceiveHandler;
				}
			}

			if (!ZSocketClient.Instance.IsConnected)
			{
				if (GUILayout.Button("Connect"))
				{
					ZSocketClient.Instance.msgHandle += MsgHandle;
					ZSocketClient.Instance.Setup("127.0.0.1", 6666);
					ZSocketClient.Instance.ConnectAsync();
				}
			}
			else
			{
				if (GUILayout.Button("Send"))
				{
					ZSocketClient.Instance.Send(Encoding.Default.GetBytes("Hello"));
				}
			}

			GUILayout.Label(_msg);
		}

		private void ReceiveHandler()
		{
			_msg += "Client Connected!\r\n";
		}

		private void MsgHandle(byte[] data)
		{
			_msg += Encoding.Default.GetString(data) + "\r\n";
		}
	}
}