using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class Debug
{
    public static bool EnableLog = true;
    public static bool EnableTime = true;

    internal static readonly Queue<LogContent> Prints = new Queue<LogContent>();
    private static GameObject _debugger;

    public static GUIMono Debugger
    {
        get
        {
            if (!_debugger)
                _debugger = new GameObject("_Debugger_", typeof (GUIMono));
            return _debugger.GetComponent<GUIMono>();
        }
    }

    private static string Time
    {
        get { return EnableTime ? (DateTime.Now.ToString("hh:mm:ss.fff") + ": ") : ""; }
    }

    public static void Log(object message)
    {
        Log(message, null);
    }

    public static void Log(object message, Object context)
    {
        Print(LogLevel.Log, message, context);
    }

    public static void LogError(object message)
    {
        LogError(message, null);
    }

    public static void LogError(object message, Object context)
    {
        Print(LogLevel.Error, message, context);
    }

    public static void LogException(Exception exception)
    {
        LogException(exception, null);
    }

    public static void LogException(Exception exception, Object context)
    {
        Print(LogLevel.Exception, exception, context);
    }

    public static void LogWarning(object message)
    {
        LogWarning(message, null);
    }

    public static void LogWarning(object message, Object context)
    {
        Print(LogLevel.Warning, message, context);
    }

    private static void Print(LogLevel level, object message, Object context)
    {
        if (EnableLog)
        {
            switch (level)
            {
                case LogLevel.Log:
                    UnityEngine.Debug.Log(Time + message, context);
                    break;
                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(Time + message, context);
                    break;
                case LogLevel.Error:
                    UnityEngine.Debug.LogError(Time + message, context);
                    break;
                case LogLevel.Exception:
                    UnityEngine.Debug.LogException((Exception) message, context);
                    break;
            }

            if (Application.isPlaying && !Application.isEditor && Debugger.enablePrint)
            {
                switch (level)
                {
                    case LogLevel.Log:
                        if (!Debugger.printLogs)
                            return;
                        break;
                    case LogLevel.Warning:
                        if (!Debugger.printWarnings)
                            return;
                        break;
                    case LogLevel.Error:
                    case LogLevel.Exception:
                        if (!Debugger.printErrors)
                            return;
                        break;
                }
                Prints.Enqueue(new LogContent(Time + message, level));

                if (Debugger.maxPrintLines < 1)
                {
                    Debugger.maxPrintLines = 1;
                }

                while (Prints.Count > Debugger.maxPrintLines)
                {
                    Prints.Dequeue();
                }
            }
        }
    }

    #region Internal

    internal class LogContent
    {
        public LogContent(string message, LogLevel level)
        {
            Message = message;
            Level = level;
        }

        public LogLevel Level { get; private set; }
        public string Message { get; private set; }
    }

    [Flags]
    internal enum LogLevel
    {
        Log = 1,
        Warning = 2,
        Error = 4,
        Exception = 8
    }

    #endregion
}

public sealed class GUIMono : MonoBehaviour
{
    private readonly Dictionary<Debug.LogLevel, GUIStyle> _styles = new Dictionary<Debug.LogLevel, GUIStyle>();

    #region Fields

    public bool enablePrint = true;
    [Range(1, 20)] public int maxPrintLines = 10;
    public bool printErrors = true;
    public bool printWarnings;
    public bool printLogs;
    public Color errorColor = Color.red;
    public Color warningColor = Color.yellow;
    public Color logColor = Color.white;

    #endregion

    private void Awake()
    {
        ReadHistory();

        GUIStyle errorStyle = new GUIStyle();
        GUIStyle warningStyle = new GUIStyle();
        GUIStyle logStyle = new GUIStyle();
        errorStyle.normal.textColor = errorColor;
        warningStyle.normal.textColor = warningColor;
        logStyle.normal.textColor = logColor;
        errorStyle.wordWrap = true;
        warningStyle.wordWrap = true;
        logStyle.wordWrap = true;
        _styles.Add(Debug.LogLevel.Log, logStyle);
        _styles.Add(Debug.LogLevel.Warning, warningStyle);
        _styles.Add(Debug.LogLevel.Error, errorStyle);
        _styles.Add(Debug.LogLevel.Exception, errorStyle);
    }

    private void ReadHistory()
    {
        string ep = PlayerPrefs.GetString("enablePrint");
        if (!string.IsNullOrEmpty(ep))
            enablePrint = HashConverter.ToObject<bool>(ep);

        string mp = PlayerPrefs.GetString("maxPrintLines");
        if (!string.IsNullOrEmpty(mp))
            maxPrintLines = HashConverter.ToObject<int>(mp);

        string pe = PlayerPrefs.GetString("printErrors");
        if (!string.IsNullOrEmpty(pe))
            printErrors = HashConverter.ToObject<bool>(pe);

        string pw = PlayerPrefs.GetString("printWarnings");
        if (!string.IsNullOrEmpty(pw))
            printWarnings = HashConverter.ToObject<bool>(pw);

        string pl = PlayerPrefs.GetString("printLogs");
        if (!string.IsNullOrEmpty(pl))
            printLogs = HashConverter.ToObject<bool>(pl);

        string ec = PlayerPrefs.GetString("errorColor");
        if (!string.IsNullOrEmpty(ec))
        {
            float[] pams = HashConverter.ToObjects<float>(ec);
            errorColor = new Color(pams[0], pams[1], pams[2], pams[3]);
        }

        string lc = PlayerPrefs.GetString("logColor");
        if (!string.IsNullOrEmpty(lc))
        {
            float[] pams = HashConverter.ToObjects<float>(lc);
            logColor = new Color(pams[0], pams[1], pams[2], pams[3]);
        }

        string wc = PlayerPrefs.GetString("warningColor");
        if (!string.IsNullOrEmpty(wc))
        {
            float[] pams = HashConverter.ToObjects<float>(wc);
            warningColor = new Color(pams[0], pams[1], pams[2], pams[3]);
        }
    }

    private void OnGUI()
    {
        if (enablePrint)
        {
            GUI.depth = 0;

            GUILayout.BeginVertical("box");
            {
                foreach (Debug.LogContent print in Debug.Prints)
                {
                    GUILayout.Label(print.Message, _styles[print.Level], GUILayout.MaxWidth(Screen.width),
                        GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(false));
                }
            }
            GUILayout.EndVertical();
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString("enablePrint", HashConverter.ToHash(enablePrint));
        PlayerPrefs.SetString("maxPrintLines", HashConverter.ToHash(maxPrintLines));
        PlayerPrefs.SetString("printErrors", HashConverter.ToHash(printErrors));
        PlayerPrefs.SetString("printWarnings", HashConverter.ToHash(printWarnings));
        PlayerPrefs.SetString("printLogs", HashConverter.ToHash(printLogs));

        PlayerPrefs.SetString("errorColor",
            HashConverter.ToHash(new[] {errorColor.r, errorColor.g, errorColor.b, errorColor.a}));
        PlayerPrefs.SetString("warningColor",
            HashConverter.ToHash(new[] {warningColor.r, warningColor.g, warningColor.b, warningColor.a}));
        PlayerPrefs.SetString("logColor",
            HashConverter.ToHash(new[] {logColor.r, logColor.g, logColor.b, logColor.a}));
    }
}

