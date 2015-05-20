using UnityEngine;

public class ReplayDemo : MonoBehaviour
{
    public TransformRecorder.RecordOptions recordOptions = TransformRecorder.RecordOptions.Transform;
    public TransformRecorder.ReplayOptions replayOptions = TransformRecorder.ReplayOptions.OneShot;

    private void OnGUI()
    {
        if (GUILayout.Button("Record"))
        {
            TransformRecorder.Record(transform, recordOptions);
            GetComponent<Rigidbody>().useGravity = true;
        }

        if (GUILayout.Button("Replay"))
        {
            TransformRecorder.Replay(transform, replayOptions);
            Destroy(GetComponent<Rigidbody>());
        }

        if (GUILayout.Button("Stop"))
        {
            TransformRecorder.Stop(transform);
        }
    }
}