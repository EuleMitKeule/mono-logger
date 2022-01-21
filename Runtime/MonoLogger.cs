using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonoLogger.Runtime
{
    public static class MonoLogger
    {
        static Dictionary<object, LogType> ObjectToLogLevel { get; } =
            new Dictionary<object, LogType>();

        public static LogType DefaultLogLevel { get; set; } = LogType.Error;

        public static void SetLogLevel(this object obj, LogType logType)
        {
#if UNITY_EDITOR

            if (!ObjectToLogLevel.ContainsKey(obj)) ObjectToLogLevel.Add(obj, logType);
            else ObjectToLogLevel[obj] = logType;
#endif
        }

        public static LogType GetLogLevel(this object obj) =>
            ObjectToLogLevel.ContainsKey(obj) ? ObjectToLogLevel[obj] : LogType.Warning;

        public static void Log(object message, LogType logType = LogType.Log)
        {
#if UNITY_EDITOR
            switch (logType)
            {
                case LogType.Log:

                    if (DefaultLogLevel == LogType.Exception ||
                        DefaultLogLevel == LogType.Error ||
                        DefaultLogLevel == LogType.Warning ||
                        DefaultLogLevel == LogType.Assert) return;

                    Debug.Log($"{message}");

                    break;
                case LogType.Assert:

                    if (DefaultLogLevel == LogType.Exception ||
                        DefaultLogLevel == LogType.Error ||
                        DefaultLogLevel == LogType.Warning) return;

                    Debug.LogAssertion($"{message}");

                    break;
                case LogType.Warning:

                    if (DefaultLogLevel == LogType.Exception ||
                        DefaultLogLevel == LogType.Error) return;

                    Debug.LogWarning($"{message}");

                    break;
                case LogType.Error:

                    if (DefaultLogLevel == LogType.Exception) return;

                    Debug.LogError($"{message}");

                    break;
                case LogType.Exception:

                    if (!(message is Exception exception)) return;

                    Debug.LogException(exception);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);

            }
#endif
        }

        public static void Log(this object obj, object message, LogType logType = LogType.Log)
        {
#if UNITY_EDITOR

            if (!ObjectToLogLevel.ContainsKey(obj)) ObjectToLogLevel.Add(obj, DefaultLogLevel);

            switch (logType)
            {
                case LogType.Log:

                    if (ObjectToLogLevel[obj] == LogType.Exception ||
                        ObjectToLogLevel[obj] == LogType.Error ||
                        ObjectToLogLevel[obj] == LogType.Warning ||
                        ObjectToLogLevel[obj] == LogType.Assert) return;

                    Debug.Log($"{obj} - {message}");

                    break;
                case LogType.Assert:

                    if (ObjectToLogLevel[obj] == LogType.Exception ||
                        ObjectToLogLevel[obj] == LogType.Error ||
                        ObjectToLogLevel[obj] == LogType.Warning) return;

                    Debug.LogAssertion($"{obj} - {message}");

                    break;
                case LogType.Warning:

                    if (ObjectToLogLevel[obj] == LogType.Exception ||
                        ObjectToLogLevel[obj] == LogType.Error) return;

                    Debug.LogWarning($"{obj} - {message}");

                    break;
                case LogType.Error:

                    if (ObjectToLogLevel[obj] == LogType.Exception) return;

                    Debug.LogError($"{obj} - {message}");

                    break;
                case LogType.Exception:

                    if (!(message is Exception exception)) return;

                    Debug.LogException(exception);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logType), logType, null);

            }
#endif
        }
    }
}
