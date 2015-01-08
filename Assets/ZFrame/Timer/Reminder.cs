using System;

namespace ZFrame.Timer
{
	public class Reminder
	{
		public string Key { get; set; }
		public ReminderType Type { get; protected set; }

		private readonly Action _onUpdate;
		private DateTime _time;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public Reminder(string key, ReminderType type, DateTime time, Action onUpdate)
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
				case ReminderType.OneTime:
				case ReminderType.EveryYear:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute && time.Hour == _time.Hour &&
					            time.Day == _time.Day && time.Month == _time.Month;
					break;
				case ReminderType.EveryMonth:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute && time.Hour == _time.Hour &&
					            time.Day == _time.Day;
					break;
				case ReminderType.EveryWeek:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute && time.Hour == _time.Hour &&
					            time.DayOfWeek == _time.DayOfWeek;
					break;
				case ReminderType.EveryDay:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute && time.Hour == _time.Hour;
					break;
				case ReminderType.EveryHour:
					canUpdate = time.Second == _time.Second && time.Minute == _time.Minute;
					break;
				case ReminderType.EveryMinute:
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
	public enum ReminderType
	{
		EveryYear,
		EveryMonth,
		EveryWeek,
		EveryDay,
		EveryHour,
		EveryMinute,
		OneTime
	}
}