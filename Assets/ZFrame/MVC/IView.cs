using System.Collections.Generic;

namespace ZFrame.MVC
{
	public interface IView
	{
		string Name { get; set; }
		EventDispatcher Dispatcher { get; set; }
		List<string> EventList { get; }
		void Init();
		void Dispose();
		void Notify(IEvent evt);
	}
}