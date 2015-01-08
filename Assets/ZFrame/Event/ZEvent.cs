namespace ZFrame.Event
{
	public class ZEvent : IEvent
	{
		public string Name { get; protected set; }
		public object Data { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ZFrame.Event.ZEvent"/> class.
		/// </summary>
		public ZEvent(string name, object data)
		{
			Name = name;
			Data = data;
		}

		public static implicit operator string(ZEvent evt)
		{
			return evt.Name;
		}
	}
}