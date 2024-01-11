using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AndroidLogger
{
    private static List<Text> subscribedTexts = new List<Text>();

    public static void SubscribeText(Text text)
    {
        if (!subscribedTexts.Contains(text))
        {
            subscribedTexts.Add(text);
            OnLogWritten += text.UpdateTextFromLog;
        }
    }

    public static void UnsubscribeText(Text text)
    {
        if (subscribedTexts.Contains(text))
        {
            subscribedTexts.Remove(text);
            OnLogWritten -= text.UpdateTextFromLog;
        }
    }

    public delegate void LogWritten(string logText);
    public static event LogWritten OnLogWritten;

    public static void WriteLog(string text)
    {
        OnLogWritten?.Invoke(text);
    }
}

public static class TextExtensions
{
    public static void UpdateTextFromLog(this Text text, string logText)
    {
        text.text = logText;
    }
}
