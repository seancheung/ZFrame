namespace ZFrame.Event
{
	public interface IEventListener
	{
		void HandleEvent(IEvent evt);
	}
}