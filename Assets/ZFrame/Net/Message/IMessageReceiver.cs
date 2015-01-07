namespace ZFrame.Net.Message
{
	public interface IMessageReceiver
	{
		void ReceiveMessage<T>(T message) where T : struct;
	}
}