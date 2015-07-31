using System;
using UnityEngine;
using ZFrame.Base.MonoBase;

public class EntrySubscriberA : IDisposable
{
    public EntrySubscriberA()
    {
        MonoEntry.Instance.UpdateHandler += Update;
    }

    private void Update()
    {
        Debug.Log(this + "=>Update");
    }

    public void Dispose()
    {
        MonoEntry.Instance.UpdateHandler -= Update;
    }
}

public class EntrySubscriberB
{
    public EntrySubscriberB()
    {
        MonoEntry.Instance.StartHandler += Start;
        MonoEntry.Instance.UpdateHandler += Update;
    }

    private void Start()
    {
        Debug.Log(this + "=>Start");
    }

    private void Update()
    {
        Debug.Log(this + "=>Update");
    }
}

public class EntrySubscriberC
{
    public EntrySubscriberC()
    {
        MonoEntry.Instance.OnEnableHandler += OnEnable;
    }

    private void OnEnable()
    {
        Debug.Log(this + "=>OnEnable");
    }
}