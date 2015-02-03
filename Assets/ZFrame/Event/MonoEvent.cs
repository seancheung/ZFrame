using System;

namespace ZFrame.EventSystem
{
	public class MonoEvent<TEnum> where TEnum : IComparable, IConvertible
	{
		public TEnum Key { get; set; }

		public MonoEventArg EventArg { get; protected set; }

		public MonoEvent(TEnum key, MonoEventArg eventArg)
		{
			Key = key;
			EventArg = eventArg;
		}

		public override string ToString()
		{
			return string.Format("Type: {0}, EventArg: {1}", Key, EventArg);
		}
	}

	public class MonoEventArg
	{
		public object Sender { get; set; }
		public object Data { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MonoEventArg"/> class.
		/// </summary>
		public MonoEventArg(object sender, object data)
		{
			Sender = sender;
			Data = data;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MonoEventArg"/> class.
		/// </summary>
		public MonoEventArg(object sender)
		{
			Sender = sender;
		}

		public override string ToString()
		{
			return string.Format("Sender: {0}, Data: {1}", Sender, Data);
		}
	}
}