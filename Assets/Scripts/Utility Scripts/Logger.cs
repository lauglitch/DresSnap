using UnityEngine;

public enum LogLevel
{
    Debug,      //print debug, warning, error and Trace
    Warning,    //print warning and error
    Error,      //print error and trace
}

public static class Logger
{
    public static LogLevel logLevel = LogLevel.Debug; // Change here

    public static void Log(LogLevel level, string message)
    {
        if (level >= logLevel)
        {
            Debug.Log(message);
        }
    }
}
