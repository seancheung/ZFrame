using System;

namespace ZFrame.Timer
{
	internal class Clock
	{
		public string Key { get; set; }
		public int Interval { get; set; }

		private readonly Action _onUpdate;
		private int _time;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public Clock(string key, int interval, Action callback)
		{
			Key = key;
			Interval = interval;
			_onUpdate = callback;
		}

		public bool Update()
		{
			_time++;
			if (_time < Interval) return false;
			if (_onUpdate != null)
			{
				_onUpdate();
				return true;
			}
			_time = 0;
			return false;
		}
	}
}