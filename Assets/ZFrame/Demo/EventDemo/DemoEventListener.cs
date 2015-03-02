using UnityEngine;
using ZFrame.EventSystem;

public class DemoEventListener : MonoEventListenr<int>
{
    private void Start()
    {
        EventHandler += OnEventHandler;
    }

    private void OnEventHandler(MonoEvent<int> monoEvent)
    {
        switch (monoEvent.Key)
        {
            case 1:
            case 2:
            case 3:
                Debug.Log(monoEvent.EventArg.Sender + " => " + monoEvent.Key + " =>" + monoEvent.Key*10);
                break;
            case 4:
            case 5:
            case 6:
                Debug.Log(monoEvent.EventArg.Sender + " => " + monoEvent.Key);
                break;
            default:
                Debug.Log(monoEvent.EventArg.Sender + " => " + monoEvent.Key + " =>" + monoEvent.Key*monoEvent.Key);
                break;
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("9"))
            MonoEventEngine<int>.Instance.QueueEvent(new MonoEvent<int>(9, new MonoEventArg(this)));
        if (GUILayout.Button("6"))
            MonoEventEngine<int>.Instance.QueueEvent(new MonoEvent<int>(6, new MonoEventArg(this)));
        if (GUILayout.Button("2"))
            MonoEventEngine<int>.Instance.QueueEvent(new MonoEvent<int>(2, new MonoEventArg(this)));

        if (GUILayout.Button("ignore"))
            MonoEventEngine<string>.Instance.QueueEvent(new MonoEvent<string>("ignore", new MonoEventArg(this)));
    }
}