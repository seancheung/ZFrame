namespace ZFrame.Event
{
	public interface IEvent
	{
		string Name { get; }
		object Data { get; }
	}
}