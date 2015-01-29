namespace ZFrame.Event
{
	public class MonoEvent : IEvent
	{
		public string Name { get; protected set; }
		object IEvent.Data { get { return EventArg; }}

		public MonoEventArg EventArg { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MonoEvent"/> class.
		/// </summary>
		public MonoEvent(string name, MonoEventArg eventArg)
		{
			Name = name;
			EventArg = eventArg;
		}

		public static implicit operator string(MonoEvent evt)
		{
			return evt.Name;
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
	}
}