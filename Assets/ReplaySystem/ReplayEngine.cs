using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplayEngine : MonoStatic<ReplayEngine>
{
    public static void StartRecord(GameObject target, RecordOptions options = RecordOptions.Transform)
    {
        var componet = target.GetComponent<RecordBehaviour>() ?? target.AddComponent<RecordBehaviour>();
        componet.isRecording = true;
        componet.isReplaying = false;
        componet.steps.Clear();
    }

    public static void StopRecord(GameObject target)
    {
        var componet = target.GetComponent<RecordBehaviour>();
        if (componet && componet.isRecording)
        {
            componet.isRecording = false;
        }
    }

    public static void StartReplay(GameObject target)
    {
        var componet = target.GetComponent<RecordBehaviour>();
        if (componet && !componet.isRecording)
        {
            componet.isReplaying = true;
        }
    }

    public static void StopReplay(GameObject target)
    {
        var componet = target.GetComponent<RecordBehaviour>();
        if (componet && componet.isReplaying)
        {
            componet.isReplaying = false;
        }
    }

    [Flags]
    public enum RecordOptions
    {
        Position = 0,
        Rotation = 1,
        Scale = 2,
        Transform = Position | Rotation | Scale,
    }

    public class Step
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}

public class RecordBehaviour : MonoBehaviour
{
    public bool isRecording;
    public bool isReplaying;
    public Queue<ReplayEngine.Step> steps = new Queue<ReplayEngine.Step>();

    private void Update()
    {
        if (isRecording && ! isReplaying)
        {
            ReplayEngine.Step step = new ReplayEngine.Step
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale
            };
            steps.Enqueue(step);
        }
        else if (isReplaying && steps.Count > 0)
        {
            var step = steps.Dequeue();
            transform.position = step.Position;
            transform.rotation = step.Rotation;
            transform.localScale = step.Scale;
        }
        else if(steps.Count == 0)
        {
            Destroy(this);
        }
    }
}