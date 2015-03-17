using System;
using System.Collections.Generic;

public static class EventCenter
{
    private static readonly Dictionary<string, Action<EventArgument>> _eventDict =
        new Dictionary<string, Action<EventArgument>>();

    private static readonly Queue<BaseEvent> _events = new Queue<BaseEvent>();

    public static void AddListener(string eventKey, Action<EventArgument> arg)
    {
        if (!_eventDict.ContainsKey(eventKey))
            _eventDict.Add(eventKey, arg);
        else
            _eventDict[eventKey] += arg;
    }

    public static void RemoveListener(string eventKey, Action<EventArgument> arg)
    {
        if (_eventDict.ContainsKey(eventKey))
            _eventDict[eventKey] -= arg;
    }

    public static void Clear()
    {
        _eventDict.Clear();
        _events.Clear();
    }

    public static void ClearEvents(string eventKey)
    {
        if (_eventDict.ContainsKey(eventKey))
            _eventDict.Remove(eventKey);
    }

    public static void Dispatch()
    {
        if (_events.Count > 0)
        {
            BaseEvent evt = _events.Dequeue();
            if (_eventDict.ContainsKey(evt.Key))
            {
                _eventDict[evt.Key].Invoke(evt.Argument);
            }
        }
    }

    public static void RaiseEvent(BaseEvent evt)
    {
        _events.Enqueue(evt);
    }
}

public class EventArgument : EventArgs
{
    public object Sender { get; protected set; }
    public object[] Argument { get; protected set; }

    public EventArgument(object sender, params object[] arguments)
    {
        Sender = sender;
        Argument = arguments;
    }
}

public class BaseEvent
{
    public string Key { get; protected set; }
    public EventArgument Argument { get; protected set; }

    public BaseEvent(string key, object sender, params object[] arguments)
    {
        Key = key;
        Argument = new EventArgument(sender, arguments);
    }
}