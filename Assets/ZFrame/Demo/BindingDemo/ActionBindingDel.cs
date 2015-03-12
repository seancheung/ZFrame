using System;
using UnityEngine;

public class ActionBindingDel : MonoBehaviour
{
    public event Action<ActionBindingDel> ActionHandler;
    public event Action<DateTime> TimeHandler;
    [HideInInspector] public float time = 5f;

    private void Update()
    {
        if (TimeHandler != null) TimeHandler.Invoke(DateTime.Now);
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (ActionHandler != null) ActionHandler.Invoke(this);
        }
    }
}