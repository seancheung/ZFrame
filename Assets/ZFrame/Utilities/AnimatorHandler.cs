using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator))]
public class AnimatorHandler : MonoBehaviour
{
    public event Action<AnimatorStateInfo, AnimatorStateInfo> StateChangedHandler;
    private readonly Dictionary<string, Action> _stateEnterActions = new Dictionary<string, Action>();
    private readonly Dictionary<string, Action> _stateExitActions = new Dictionary<string, Action>();

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
            foreach (KeyValuePair<string, Action> exitAction in _stateExitActions)
            {
                if (lastState.IsName(exitAction.Key))
                    exitAction.Value.Invoke();
            }

            foreach (KeyValuePair<string, Action> enterAction in _stateEnterActions)
            {
                if (newState.IsName(enterAction.Key))
                    enterAction.Value.Invoke();
            }
        };
    }

    private void Update()
    {
        AnimatorStateInfo info = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
#if UNITY_5
        if (info.shortNameHash != _info.shortNameHash)
#else
        if (info.nameHash != _info.nameHash)
#endif
        {
            OnStateChangedHandler(_info, info);
            _info = info;
        }
    }

    public void RegisterStateEnterHandler(string stateName, Action action)
    {
        if (_stateEnterActions.ContainsKey(stateName))
            _stateEnterActions[stateName] += action;
        _stateEnterActions.Add(stateName, action);
    }

    public void RegisterStateExitHandler(string stateName, Action action)
    {
        if (_stateExitActions.ContainsKey(stateName))
            _stateExitActions[stateName] += action;
        _stateExitActions.Add(stateName, action);
    }
}