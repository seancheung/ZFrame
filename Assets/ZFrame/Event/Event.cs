namespace ZFrame.Event
{
	public class Event : IEvent
	{
		public string Name { get; protected set; }
		public object Data { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public Event(string name, object data)
		{
			Name = name;
			Data = data;
		}

		public static implicit operator string(Event evt)
		{
			return evt.Name;
		}
	}

}