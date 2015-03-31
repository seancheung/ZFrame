using UnityEngine;

public class ReplayDemo : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("Record"))
        {
            ReplayEngine.StartRecord(gameObject);
        }

        if (GUILayout.Button("Stop Record"))
        {
            ReplayEngine.StopRecord(gameObject);
        }

        if (GUILayout.Button("Replay"))
        {
            ReplayEngine.StartReplay(gameObject);
        }

        if (GUILayout.Button("Stop Replay"))
        {
            ReplayEngine.StopReplay(gameObject);
        }
    }
}