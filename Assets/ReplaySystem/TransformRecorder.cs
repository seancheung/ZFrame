using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
    [Flags]
    public enum RecordOptions
    {
        Position = 1,
        Rotation = 2,
        Scale = 4,
        Transform = Position | Rotation | Scale,
    }

    public enum ReplayOptions
    {
        OneShot,
        Loop,
        PingPongOnce,
        PingPongLoop
    }

    private class TransformInfo
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformInfo(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }

    [SerializeField] private RecordOptions recordOptions;
    [SerializeField] private ReplayOptions replayOptions;

    private List<TransformInfo> infos = new List<TransformInfo>();

    public static void Record(Transform target, RecordOptions options = RecordOptions.Transform)
    {
        TransformRecorder componet = target.GetComponent<TransformRecorder>() ??
                                         target.gameObject.AddComponent<TransformRecorder>();
        componet.StopAllCoroutines();
        componet.StartCoroutine(componet.Record(options));
    }

    public static void Replay(Transform target, ReplayOptions options = ReplayOptions.OneShot)
    {
        TransformRecorder componet = target.GetComponent<TransformRecorder>();
        if (componet)
        {
            componet.StopAllCoroutines();
            componet.StartCoroutine(componet.Replay(options));
        }
    }

    public static void Stop(Transform target)
    {
        TransformRecorder componet = target.GetComponent<TransformRecorder>();
        if (componet)
            componet.StopAllCoroutines();
    }

    private IEnumerator Record(RecordOptions options)
    {
        infos.Clear();
        recordOptions = options;
        while (true)
        {
            infos.Add(new TransformInfo(transform.position, transform.rotation, transform.localScale));
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Replay(ReplayOptions options)
    {
        replayOptions = options;

        switch (replayOptions)
        {
            case ReplayOptions.OneShot:
                foreach (TransformInfo info in infos)
                {
                    ApplyInfo(info);
                    yield return new WaitForEndOfFrame();
                }
                break;
            case ReplayOptions.Loop:
                while (true)
                {
                    foreach (TransformInfo info in infos)
                    {
                        ApplyInfo(info);
                        yield return new WaitForEndOfFrame();
                    }
                }
            case ReplayOptions.PingPongOnce:
                foreach (TransformInfo info in infos)
                {
                    ApplyInfo(info);
                    yield return new WaitForEndOfFrame();
                }
                infos.Reverse();
                foreach (TransformInfo info in infos)
                {
                    ApplyInfo(info);
                    yield return new WaitForEndOfFrame();
                }
                break;
            case ReplayOptions.PingPongLoop:
                while (true)
                {
                    foreach (TransformInfo info in infos)
                    {
                        ApplyInfo(info);
                        yield return new WaitForEndOfFrame();
                    }
                    infos.Reverse();
                    foreach (TransformInfo info in infos)
                    {
                        ApplyInfo(info);
                        yield return new WaitForEndOfFrame();
                    }
                }
            default:
                throw new ArgumentOutOfRangeException("options");
        }
    }

    private void ApplyInfo(TransformInfo info)
    {
        if ((recordOptions & RecordOptions.Position) == RecordOptions.Position)
            transform.position = info.position;
        if ((recordOptions & RecordOptions.Rotation) == RecordOptions.Rotation)
            transform.rotation = info.rotation;
        if ((recordOptions & RecordOptions.Scale) == RecordOptions.Scale)
            transform.localScale = info.scale;
    }
}