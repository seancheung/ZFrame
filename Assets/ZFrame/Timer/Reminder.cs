using System;

namespace ZFrame.Timer
{
	public class Reminder
	{
		public string Key { get; set; }
		public ReminderType Type { get; protected set; }

		private readonly TickCallback _onUpdate;
		private DateTime _time;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public Reminder(string key, ReminderType type, DateTime time, TickCallback onUpdate)
		{
			Key = key;
			Type = type;
			_time = time;
			_onUpdate = onUpdate;
		}

		public bool Update(DateTime time)
		{
			bool canUpdate;
			switch (Type)
			{
				case ReminderType.Year:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute && time.Hour == _time.Hour &&
					            time.Day == _time.Day && time.Month == _time.Month;
					break;
				case ReminderType.Month:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute && time.Hour == _time.Hour &&
					            time.Day == _time.Day;
					break;
				case ReminderType.Week:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute && time.Hour == _time.Hour &&
					            time.DayOfWeek == _time.DayOfWeek;
					break;
				case ReminderType.Day:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute && time.Hour == _time.Hour;
					break;
				case ReminderType.Hour:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute;
					break;
				case ReminderType.Minute:
					canUpdate = time.Second == _time.Second;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			if (canUpdate && _onUpdate != null)
			{
				_onUpdate();
				return true;
			}

			return false;
		}
	}
}