using UnityEngine;
using ZFrame.Base.MonoBase;

public class EntryDemo : MonoBehaviour
{
    private EntrySubscriberA a;
    private EntrySubscriberB b;
    private EntrySubscriberC c;

    private void Start()
    {
        a = new EntrySubscriberA();
        b = new EntrySubscriberB();
        c = new EntrySubscriberC();

        MonoEntry.Instance.UpdateHandler += Do; // anonymous subscribe is not allowed!
        MonoEntry.Instance.UpdateHandler += () => Debug.Log(this + "=>Update"); 
    }

    void Do()
    {
        Debug.Log(this+"=>Update");
    }

    private void OnDestroy()
    {
        a.Dispose();
        b = null;
        c = null;
    }
}