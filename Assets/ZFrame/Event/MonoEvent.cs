namespace ZFrame.EventSystem
{
	public class MonoEvent
	{
		public string Name { get; protected set; }

		public MonoEventArg EventArg { get; protected set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:MonoEvent"/> class.
		/// </summary>
		public MonoEvent(string name, MonoEventArg eventArg)
		{
			Name = name;
			EventArg = eventArg;
		}

		public override string ToString()
		{
			return string.Format("Name: {0}, EventArg: {1}", Name, EventArg);
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

		public override string ToString()
		{
			return string.Format("Sender: {0}, Data: {1}", Sender, Data);
		}
	}
}