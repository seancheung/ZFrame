using System;
using System.Collections.Generic;
using UnityEngine;

public class ReplayEngine : MonoStatic<ReplayEngine>
{
    public static void StartRecord(GameObject target, RecordOptions options = RecordOptions.Transform)
    {
        RecordBehaviour componet = target.GetComponent<RecordBehaviour>() ?? target.AddComponent<RecordBehaviour>();
        componet.SendMessage("StartRecord", options);
    }

    public static void StopRecord(GameObject target)
    {
        RecordBehaviour componet = target.GetComponent<RecordBehaviour>();
        if (componet)
        {
            componet.SendMessage("StopRecord");
        }
    }

    public static void StartReplay(GameObject target, ReplayOptions options = ReplayOptions.OneShot)
    {
        RecordBehaviour componet = target.GetComponent<RecordBehaviour>();
        if (componet)
        {
            componet.SendMessage("StartReplay", options);
        }
    }

    public static void StopReplay(GameObject target)
    {
        RecordBehaviour componet = target.GetComponent<RecordBehaviour>();
        if (componet)
        {
            componet.SendMessage("StopReplay");
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

    public enum ReplayOptions
    {
        OneShot,
        Loop,
        PingPongOnce,
        PingPongLoop
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
    private ReplayEngine.ReplayOptions replayOptions;
    private ReplayEngine.RecordOptions recordOptions;
    private bool isRecording;
    private bool isReplaying;
    private List<ReplayEngine.Step> steps = new List<ReplayEngine.Step>();
    private int index;
    private bool isForward = true;

    private void Update()
    {
        if (isRecording)
        {
            ReplayEngine.Step step = new ReplayEngine.Step
            {
                Position = transform.position,
                Rotation = transform.rotation,
                Scale = transform.localScale
            };
            steps.Add(step);
        }
        else if (isReplaying)
        {
            ReplayEngine.Step step;

            switch (replayOptions)
            {
                case ReplayEngine.ReplayOptions.Loop:
                {
                    index++;
                    if (index == steps.Count)
                        index = 0;
                }
                    break;
                case ReplayEngine.ReplayOptions.PingPongOnce:
                {
                    if (isForward)
                    {
                        index++;
                        if (index == steps.Count - 1)
                        {
                            isForward = false;
                        }
                    }
                    else
                    {
                        index--;
                        if (index == 0)
                        {
                            isReplaying = false;
                        }
                    }
                }
                    break;
                case ReplayEngine.ReplayOptions.PingPongLoop:
                {
                    if (isForward)
                    {
                        index++;
                        if (index == steps.Count - 1)
                        {
                            isForward = false;
                        }
                    }
                    else
                    {
                        index--;
                        if (index == 0)
                        {
                            isForward = true;
                        }
                    }
                }
                    break;
                default:
                {
                    if (index < steps.Count - 1)
                        index++;
                    else
                        isReplaying = false;
                }
                    break;
            }

            step = steps[index];

            if ((recordOptions & ReplayEngine.RecordOptions.Position) == ReplayEngine.RecordOptions.Position)
                transform.position = step.Position;
            if ((recordOptions & ReplayEngine.RecordOptions.Rotation) == ReplayEngine.RecordOptions.Rotation)
                transform.rotation = step.Rotation;
            if ((recordOptions & ReplayEngine.RecordOptions.Scale) == ReplayEngine.RecordOptions.Scale)
                transform.localScale = step.Scale;
        }
        else if (steps.Count == 0)
        {
            Destroy(this);
        }
    }

    private void StartRecord(ReplayEngine.RecordOptions options)
    {
        recordOptions = options;
        isRecording = true;
    }

    private void StopRecord()
    {
        isRecording = false;
    }

    private void StartReplay(ReplayEngine.ReplayOptions options)
    {
        replayOptions = options;
        isReplaying = true;
    }

    private void StopReplay()
    {
        isReplaying = false;
    }

    private void Clear()
    {
        steps.Clear();
    }
}