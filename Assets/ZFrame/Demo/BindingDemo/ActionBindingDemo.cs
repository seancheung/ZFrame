using System;
using UnityEngine;

public class ActionBindingDemo : MonoBehaviour
{
    public DateTime Time { get; private set; }

    public void DestroyAct(ActionBindingDel del)
    {
        Destroy(del);
    }

    public void TimeAct(DateTime time)
    {
        Time = time;
    }
}