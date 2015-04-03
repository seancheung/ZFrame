using System;
using UnityEngine;

public class MotionController : MonoBehaviour
{
    public Transform target;
    public MotionType type;
    public MoveOption moveOption;

    public void Execute()
    {
        Move();
    }

    private void Move()
    {
        switch (moveOption.moveType)
        {
            case MoveType.ToTransform:
                target.position = Vector3.MoveTowards(target.position, moveOption.targetTransform.position,
                    moveOption.speed*Time.deltaTime);
                break;
            case MoveType.ToPosition:
                target.position = Vector3.MoveTowards(target.position, moveOption.targetPostion,
                    moveOption.speed*Time.deltaTime);
                break;
            case MoveType.AlongDirection:
                target.Translate(moveOption.targetDirect.normalized*moveOption.speed*Time.deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public enum MotionType
    {
        Position,
        Rotation,
        Scale
    }

    public enum MoveType
    {
        ToTransform,
        ToPosition,
        AlongDirection,
    }

    [Serializable]
    public class MoveOption
    {
        public MoveType moveType;
        public float speed;
        public Transform targetTransform;
        public Vector3 targetPostion;
        public Vector3 targetDirect;
    }

    [Serializable]
    public class RotateOption
    {
        public Vector3 speed;
        public Transform targetTransform;
        public Vector3 targetPostion;
    }
}