using System;

namespace ZFrame.EventSystem
{
	public class MonoEvent<TEnum> : MonoEvent where TEnum : IComparable, IConvertible
	{
		public new TEnum Key
		{
			get { return (TEnum) base.Key; }
			protected set { base.Key = value; }
		}

		public MonoEvent(TEnum key, MonoEventArg eventArg) : base(key, eventArg)
		{
			Key = key;
			EventArg = eventArg;
		}

	}

	public class MonoEvent
	{
		public object Key { get; protected set; }

		public MonoEventArg EventArg { get; protected set; }

		public MonoEvent(object key, MonoEventArg eventArg)
		{
			Key = key;
			EventArg = eventArg;
		}

		public override string ToString()
		{
			return string.Format("Key: {0}, EventArg: {1}", Key, EventArg);
		}
	}
}