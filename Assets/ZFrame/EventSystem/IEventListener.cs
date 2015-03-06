namespace ZFrame.EventSystem
{
	public interface IEventListener
	{
		void HandleEvent(IEvent evt);
	}
}