namespace ZFrame.Event
{
	public class ZEvent : IEvent
	{
		public string Name { get; protected set; }
		object IEvent.Data { get { return EventArg; }}

		public ZEventArg EventArg { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ZEvent"/> class.
		/// </summary>
		public ZEvent(string name, ZEventArg eventArg)
		{
			Name = name;
			EventArg = eventArg;
		}

		public static implicit operator string(ZEvent evt)
		{
			return evt.Name;
		}
	}

	public class ZEventArg
	{
		public object Sender { get; set; }
		public object Data { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ZEventArg"/> class.
		/// </summary>
		public ZEventArg(object sender, object data)
		{
			Sender = sender;
			Data = data;
		}
	}
}