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
		private static MonoDebugger _monoDebugger;
		private static ZDebugger _zDebugger;

		private static IDebugger Instance
		{
			get
			{
				if (!Application.isPlaying) return _zDebugger ?? (_zDebugger = new ZDebugger());

				if (!_monoDebugger)
					_monoDebugger = new GameObject(typeof (ZDebug).ToString(), typeof (MonoDebugger)).GetComponent<MonoDebugger>();
				return _monoDebugger;
			}
		}

		private static readonly Queue<LogContent> Prints = new Queue<LogContent>();

		private static string Time
		{
			get { return EnableTime ? (DateTime.Now.ToString("hh:mm:ss.fff") + ": ") : ""; }
		}

		/// <summary>
		/// Enable log
		/// </summary>
		public static bool EnableLog
		{
			get { return Instance.EnableLog; }
			set { Instance.EnableLog = value; }
		}

		/// <summary>
		/// Enable GUI log
		/// </summary>
		public static bool EnablePrint
		{
			get { return Instance.EnablePrint; }
			set { Instance.EnablePrint = value; }
		}

		/// <summary>
		/// Add time when print common log or GUI log
		/// </summary>
		public static bool EnableTime
		{
			get { return Instance.EnableTime; }
			set { Instance.EnableTime = value; }
		}

		/// <summary>
		/// Max GUI log lines. Earlier lines will be erased. 
		/// </summary>
		public static int MaxPrintLines
		{
			get { return Instance.MaxPrintLines; }
			set { Instance.MaxPrintLines = value; }
		}

		/// <summary>
		/// Print errors on screen
		/// </summary>
		public static bool PrintErrors
		{
			get { return Instance.PrintErrors; }
			set { Instance.PrintErrors = value; }
		}

		/// <summary>
		/// Print warnings on screen
		/// </summary>
		public static bool PrintWarnings
		{
			get { return Instance.PrintWarnings; }
			set { Instance.PrintWarnings = value; }
		}

		/// <summary>
		/// Print logs on screen
		/// </summary>
		public static bool PrintLogs
		{
			get { return Instance.PrintLogs; }
			set { Instance.PrintLogs = value; }
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
				Prints.Enqueue(new LogContent(Time + message, level));

				if (MaxPrintLines < 1)
				{
					MaxPrintLines = 1;
				}

				while (Prints.Count > MaxPrintLines)
				{
					Prints.Dequeue();
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

		private class ZDebugger : IDebugger
		{
			#region Implementation of IDebugger

			public bool EnableLog { get; set; }
			public bool EnablePrint { get; set; }
			public bool EnableTime { get; set; }
			public int MaxPrintLines { get; set; }
			public bool PrintErrors { get; set; }
			public bool PrintWarnings { get; set; }
			public bool PrintLogs { get; set; }
			public Color ErrorColor { get; set; }
			public Color WarningColor { get; set; }
			public Color LogColor { get; set; }

			#endregion

			/// <summary>
			/// Initializes a new instance of the <see cref="T:ZDebugger"/> class.
			/// </summary>
			public ZDebugger()
			{
				EnableLog = true;
			}
		}

		private interface IDebugger
		{
			bool EnableLog { get; set; }
			bool EnablePrint { get; set; }
			bool EnableTime { get; set; }
			int MaxPrintLines { get; set; }
			bool PrintErrors { get; set; }
			bool PrintWarnings { get; set; }
			bool PrintLogs { get; set; }
			Color ErrorColor { get; set; }
			Color WarningColor { get; set; }
			Color LogColor { get; set; }
		}

		private class MonoDebugger : MonoBehaviour, IDebugger
		{
			private readonly Dictionary<LogLevel, GUIStyle> _styles = new Dictionary<LogLevel, GUIStyle>();

			private void Awake()
			{
				DontDestroyOnLoad(this);

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

			#region Fields

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

			#endregion

			public bool EnableLog
			{
				get { return enableLog; }
				set { enableLog = value; }
			}

			public bool EnablePrint
			{
				get { return enablePrint; }
				set { enablePrint = value; }
			}

			public bool EnableTime
			{
				get { return enableTime; }
				set { enableTime = value; }
			}

			public int MaxPrintLines
			{
				get { return maxPrintLines; }
				set { maxPrintLines = value; }
			}

			public bool PrintErrors
			{
				get { return printErrors; }
				set { printErrors = value; }
			}

			public bool PrintWarnings
			{
				get { return printWarnings; }
				set { printWarnings = value; }
			}

			public bool PrintLogs
			{
				get { return printLogs; }
				set { printLogs = value; }
			}

			public Color ErrorColor
			{
				get { return errorColor; }
				set { errorColor = value; }
			}

			public Color WarningColor
			{
				get { return warningColor; }
				set { warningColor = value; }
			}

			public Color LogColor
			{
				get { return logColor; }
				set { logColor = value; }
			}

#if LOGGUI
			private void OnGUI()
			{
				if (enablePrint)
				{
					foreach (LogContent print in Prints)
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

/**/