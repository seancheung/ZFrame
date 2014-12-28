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
			GameEngine.Instance.Init();
			AdvancedTimer.Instance.Init();
		}
	}
}