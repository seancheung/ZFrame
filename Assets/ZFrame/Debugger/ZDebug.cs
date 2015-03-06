using System;
using System.Collections.Generic;
using UnityEngine;
using ZFrame.Base.MonoBase;

namespace ZFrame.Debugger
{
    public sealed class ZDebug : MonoSingleton<ZDebug>
    {
        public DebugConfig Config;

        private readonly Queue<LogContent> _logs = new Queue<LogContent>();
        private readonly Dictionary<LogType, GUIStyle> _styles = new Dictionary<LogType, GUIStyle>();
        private Vector2 _scrollPos;

        private string Time
        {
            get { return Config.enableTime ? (DateTime.Now.ToString("hh:mm:ss.fff") + ": ") : ""; }
        }

        private void Start()
        {
            _styles.Add(LogType.Log, Config.logStyle);
            _styles.Add(LogType.Warning, Config.warningStyle);
            _styles.Add(LogType.Error, Config.errorStyle);
            _styles.Add(LogType.Exception, Config.exceptionStyle);
        }

        private void OnGUI()
        {
            if (Config.enableDebug)
            {
                GUI.depth = 0;

                _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.MaxHeight(Screen.height));
                {
                    GUILayout.BeginVertical("box");
                    {
                        foreach (LogContent print in _logs)
                        {
                            GUILayout.Label("<b>" + print.Message + "</b>", _styles[print.Type],
                                GUILayout.MaxWidth(Screen.width));
                            if (print.Type != LogType.Log && !string.IsNullOrEmpty(print.Trace))
                                GUILayout.Label("<i>" + print.Trace + "</i>", _styles[print.Type],
                                    GUILayout.MaxWidth(Screen.width));
                        }
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndScrollView();
            }
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    if (!Config.enableErrors)
                        return;
                    break;
                case LogType.Warning:
                    if (!Config.enableWarnings)
                        return;
                    break;
                case LogType.Log:
                    if (!Config.enableLogs)
                        return;
                    break;
                case LogType.Exception:
                    if (!Config.enableExceptions)
                        return;
                    break;
                default:
                    return;
            }
            _logs.Enqueue(Config.enableTrace
                ? new LogContent(Time + logString, type, stackTrace)
                : new LogContent(Time + logString, type));

            if (Config.maxDebugLines < 1)
            {
                Config.maxDebugLines = 1;
            }
            while (_logs.Count > Config.maxDebugLines)
            {
                _logs.Dequeue();
            }
        }

        #region Internal

        private class LogContent
        {
            public LogContent(string message, LogType type)
            {
                Message = message;
                Type = type;
            }

            public LogContent(string message, LogType type, string trace)
            {
                Message = message;
                Type = type;
                Trace = trace;
            }

            public LogType Type { get; private set; }
            public string Message { get; private set; }
            public string Trace { get; private set; }
        }

        #endregion
    }
}