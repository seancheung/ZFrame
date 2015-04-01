using UnityEngine;

public class ReplayDemo : MonoBehaviour
{
    public ReplayEngine.RecordOptions recordOptions = ReplayEngine.RecordOptions.Transform;
    public ReplayEngine.ReplayOptions replayOptions = ReplayEngine.ReplayOptions.OneShot;

    private void OnGUI()
    {
        if (GUILayout.Button("Record"))
        {
            ReplayEngine.StartRecord(gameObject, recordOptions);
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (GUILayout.Button("Stop Record"))
        {
            ReplayEngine.StopRecord(gameObject);
        }

        if (GUILayout.Button("Replay"))
        {
            ReplayEngine.StartReplay(gameObject, replayOptions);
            Destroy(GetComponent<Rigidbody>());
        }

        if (GUILayout.Button("Stop Replay"))
        {
            ReplayEngine.StopReplay(gameObject);
        }
    }
}