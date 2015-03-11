using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class AnimatorHandler : MonoBehaviour
{
    public event Action<AnimatorStateInfo, AnimatorStateInfo> StateChangedHandler;
    private readonly Dictionary<int, Action> _stateEnterActions = new Dictionary<int, Action>();
    private readonly Dictionary<int, Action> _stateExitActions = new Dictionary<int, Action>();

    protected virtual void OnStateChangedHandler(AnimatorStateInfo lastState, AnimatorStateInfo newState)
    {
        Action<AnimatorStateInfo, AnimatorStateInfo> handler = StateChangedHandler;
        if (handler != null) handler(lastState, newState);
    }

    private AnimatorStateInfo _info;

    private void Awake()
    {
        StateChangedHandler += (lastState, newState) =>
        {
            if (_stateExitActions.ContainsKey(lastState.fullPathHash))
                _stateExitActions[lastState.fullPathHash].Invoke();

            if (_stateEnterActions.ContainsKey(newState.fullPathHash))
                _stateEnterActions[lastState.fullPathHash].Invoke();
        };
    }

    private void Update()
    {
        AnimatorStateInfo info = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (info.shortNameHash != _info.shortNameHash)
        {
            OnStateChangedHandler(_info, info);
            _info = info;
        }
    }

    public void RegisterStateEnterHandler(string stateName, Action action)
    {
        if (_stateEnterActions.ContainsKey(stateName.GetHashCode()))
            _stateEnterActions[stateName.GetHashCode()] += action;
        _stateEnterActions.Add(stateName.GetHashCode(), action);
    }

    public void RegisterStateExitHandler(string stateName, Action action)
    {
        if (_stateExitActions.ContainsKey(stateName.GetHashCode()))
            _stateExitActions[stateName.GetHashCode()] += action;
        _stateExitActions.Add(stateName.GetHashCode(), action);
    }
}