/*
 * build cmd 
 * "...\Unity4.5\Editor\Data\MonoBleedingEdge\lib\mono\2.0\mcs.exe" -r:"...\Unity4.5\Editor\Data\PlaybackEngines\metrosupport\Managed\UnityEngine.dll" -target:library ...\ZFrame\Assets\ZFrame\Debugger\ZDebug.cs
 *
 * /

/* 

#define LOGGUI

using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ZFrame.Debugger
{
	public static class ZDebug
	{
		private static readonly ZDebugger Instance =
			new GameObject(typeof (ZDebug).ToString(), typeof (ZDebugger)).GetComponent<ZDebugger>();

		private static readonly Queue<LogContent> prints = new Queue<LogContent>();

		private static string Time
		{
			get { return EnableTime ? (DateTime.Now.ToString("hh:mm:ss.fff") + ": ") : ""; }
		}

		/// <summary>
		/// Enable log
		/// </summary>
		public static bool EnableLog
		{
			get { return Instance.enableLog; }
			set { Instance.enableLog = value; }
		}

		/// <summary>
		/// Enable GUI log
		/// </summary>
		public static bool EnablePrint
		{
			get { return Instance.enablePrint; }
			set { Instance.enablePrint = value; }
		}

		/// <summary>
		/// Add time when print common log or GUI log
		/// </summary>
		public static bool EnableTime
		{
			get { return Instance.enableTime; }
			set { Instance.enableTime = value; }
		}

		/// <summary>
		/// Max GUI log lines. Earlier lines will be erased. 
		/// </summary>
		public static int MaxPrintLines
		{
			get { return Instance.maxPrintLines; }
			set { Instance.maxPrintLines = value; }
		}

		/// <summary>
		/// Print errors on screen
		/// </summary>
		public static bool PrintErrors
		{
			get { return Instance.printErrors; }
			set { Instance.printErrors = value; }
		}

		/// <summary>
		/// Print warnings on screen
		/// </summary>
		public static bool PrintWarnings
		{
			get { return Instance.printWarnings; }
			set { Instance.printWarnings = value; }
		}

		/// <summary>
		/// Print logs on screen
		/// </summary>
		public static bool PrintLogs
		{
			get { return Instance.printLogs; }
			set { Instance.printLogs = value; }
		}

		/// <summary>
		/// Log message
		/// </summary>
		/// <param name="message"></param>
		public static void Log(object message)
		{
			Log(message, null);
		}

		/// <summary>
		/// Log message
		/// </summary>
		/// <param name="message"></param>
		/// <param name="context"></param>
		public static void Log(object message, Object context)
		{
			if (EnableLog)
			{
				Debug.Log(Time + message, context);
			}
			if (PrintLogs)
			{
				LogGUI(message, LogLevel.Log);
			}
		}

		/// <summary>
		/// Log error message
		/// </summary>
		/// <param name="message"></param>
		public static void LogError(object message)
		{
			LogError(message, null);
		}

		/// <summary>
		/// Log error message
		/// </summary>
		/// <param name="message"></param>
		/// <param name="context"></param>
		public static void LogError(object message, Object context)
		{
			if (EnableLog)
			{
				Debug.LogError(Time + message, context);
			}
			if (PrintErrors)
			{
				LogGUI(message, LogLevel.Error);
			}
		}

		/// <summary>
		/// Log warning message
		/// </summary>
		/// <param name="message"></param>
		public static void LogWarning(object message)
		{
			LogWarning(message, null);
		}

		/// <summary>
		/// Log warning message
		/// </summary>
		/// <param name="message"></param>
		/// <param name="context"></param>
		public static void LogWarning(object message, Object context)
		{
			if (EnableLog)
			{
				Debug.LogWarning(Time + message, context);
			}
			if (PrintWarnings)
			{
				LogGUI(message, LogLevel.Warning);
			}
		}

		private static void LogGUI(object message, LogLevel level)
		{
			if (EnablePrint)
			{
				prints.Enqueue(new LogContent(Time + message, level));

				if (MaxPrintLines < 1)
				{
					MaxPrintLines = 1;
				}

				while (prints.Count > MaxPrintLines)
				{
					prints.Dequeue();
				}
			}
			else if (EnableLog)
			{
				Log(message);
			}
		}

		///// <summary>
		///// Print message on screen
		///// </summary>
		///// <param name="message"></param>
		//public static void LogGUI(object message)
		//{
		//	LogGUI(message, LogLevel.Log);
		//}

		private class ZDebugger : MonoBehaviour
		{
			private readonly Dictionary<LogLevel, GUIStyle> _styles = new Dictionary<LogLevel, GUIStyle>();

			private void Awake()
			{
				DontDestroyOnLoad(Instance);

				GUIStyle errorStyle = new GUIStyle();
				GUIStyle warningStyle = new GUIStyle();
				GUIStyle logStyle = new GUIStyle();
				errorStyle.normal.textColor = errorColor;
				warningStyle.normal.textColor = warningColor;
				logStyle.normal.textColor = logColor;
				_styles.Add(LogLevel.Log, logStyle);
				_styles.Add(LogLevel.Warning, warningStyle);
				_styles.Add(LogLevel.Error, errorStyle);
			}

			public bool enableLog = true;
			public bool enablePrint = true;
			public bool enableTime = true;
			public int maxPrintLines = 20;
			public bool printErrors = true;
			public bool printWarnings;
			public bool printLogs;
			public Color errorColor = Color.red;
			public Color warningColor = Color.yellow;
			public Color logColor = Color.white;


#if LOGGUI
			private void OnGUI()
			{
				if (enablePrint)
				{
					foreach (LogContent print in prints)
					{
						GUILayout.Label(print.Message, _styles[print.Level]);
					}
				}
			}
#endif
		}

		[Flags]
		private enum LogLevel
		{
			Log = 1,
			Warning = 2,
			Error = 4
		}

		private class LogContent
		{
			public LogLevel Level { get; private set; }
			public string Message { get; private set; }

			public LogContent(string message, LogLevel level)
			{
				Message = message;
				Level = level;
			}
		}
	}
}
 */