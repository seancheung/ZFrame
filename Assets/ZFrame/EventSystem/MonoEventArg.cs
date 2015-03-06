namespace ZFrame.EventSystem
{
	public class MonoEventArg
	{
		public object Sender { get; set; }
		public object Data { get; set; }

		public MonoEventArg(object sender, object data)
		{
			Sender = sender;
			Data = data;
		}

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