namespace ZFrame.EventSystem
{
	public interface IEvent
	{
		string Name { get; }
		object Data { get; }
	}
}