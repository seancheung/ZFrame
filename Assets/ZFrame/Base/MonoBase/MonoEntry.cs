using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ZFrame.Base.MonoBase
{
    public sealed class MonoEntry : MonoSingleton<MonoEntry>
    {
        private Dictionary<string, List<Action>> invokers = new Dictionary<string, List<Action>>();

        public event Action AwakeHandler
        {
            add { TryAdd("Awake", value); }
            remove { TryRemove("Awake", value); }
        }

        public event Action StartHandler
        {
            add { TryAdd("Start", value); }
            remove { TryRemove("Start", value); }
        }

        public event Action OnEnableHandler
        {
            add { TryAdd("OnEnable", value); }
            remove { TryRemove("OnEnable", value); }
        }

        public event Action UpdateHandler
        {
            add { TryAdd("Update", value); }
            remove { TryRemove("Update", value); }
        }

        private void Awake()
        {
            TryInvoke("Awake");
        }

        private void Start()
        {
            TryInvoke("Start");
        }

        private void OnEnable()
        {
            TryInvoke("OnEnable");
        }

        private void Update()
        {
            TryInvoke("Update");
        }

        private void TryAdd(string key, Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (Regex.IsMatch(action.Method.Name, @"\<|\>"))
                throw new ArgumentException("Anonymous method is not allowed", "action");

            if (!invokers.ContainsKey(key))
                invokers.Add(key, new List<Action>());
            if (!invokers[key].Contains(action))
                invokers[key].Add(action);
        }

        private void TryRemove(string key, Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (invokers.ContainsKey(key) && invokers[key].Contains(action))
                invokers[key].Remove(action);
        }

        private void TryInvoke(string key)
        {
            if (invokers.ContainsKey(key))
                lock (invokers)
                    invokers[key].ForEach(a => SafeInvoke(key, a));
        }

        private void SafeInvoke(string key, Action action)
        {
            if (Equals(action.Target, null) || action.Target == null || action.Target.Equals(null))
            {
                Debug.LogError(key);
                TryRemove(key, action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}