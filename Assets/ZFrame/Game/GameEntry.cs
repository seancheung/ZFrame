using UnityEngine;
using ZFrame.Timer;

namespace ZFrame
{
	/// <summary>
	/// This is the game entry class
	/// </summary>
	public class GameEntry : MonoBehaviour
	{
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
			CoroutineTimer.Instance.Init();
			
		}

		//void OnGUI()
		//{
		//	if (GUILayout.Button("Connect"))
		//	{

		//		if (!ZSocketClient.Instance.IsConnected)
		//		{
		//			Debug.Log("setting");
		//			ZSocketClient.Instance.Setup("127.0.0.1", 6666);
		//			ZSocketClient.Instance.ConnectAsync();

		//		}
		//		else
		//		{
		//			Debug.Log("sending");
		//			ZSocketClient.Instance.Send(BitConverter.GetBytes(1));
		//		}

		//	}
		//}
	}
}