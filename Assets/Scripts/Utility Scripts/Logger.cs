using System;
using System.Collections.Generic;
using UnityEngine;

public enum LogLevel
{
    Info,       //print debug, success, warning, error and test
    Warning,    //print warning, success, error and test
    Success,    //print success, error and test 
    Error,      //print error and test
    Test,       //print test (ONLY USE WHEN DEVELOPING)
    DeepTest,   //print test
}


public static class Logger
{
    public static List<LogLevel> onlyShow = new List<LogLevel>{ 
        //LogLevel.Info,
        //LogLevel.Warning,
        LogLevel.Success,
        LogLevel.Error,
        LogLevel.Test,
        LogLevel.DeepTest,
    };

    public static void Log(LogLevel level, string message)
    {
        if (onlyShow.Contains(level))
        {
            Debug.Log(level + " - " + message);
        }   
    }

    public static void SeveralLogs(LogLevel level, List<String> messages)
    {
        String allMessages= "";
        foreach (String message in messages)
        {
            allMessages += "\n" + message;
            Debug.Log(message);
        }
    }

    public static void GameObjectString(LogLevel level, GameObject gameobject)
    {
        Debug.Log(gameobject.name);
    }

    public static string GarmentString(Garment garment)
    {
        return "Garment ID=" + garment.GarmentID + ", Name=" + garment.Name + ", Type=" + garment.Type + ", ImagePath=" + garment.ImagePath;
    }

    public static string OutfitString(Outfit outfit)
    {
        return "Outfit ID=" + outfit.OutfitID + ", TopID=" + outfit.TopID + ", BottomID=" + outfit.BottomID + ", ShoesID=" + outfit.ShoesID;
    }
}
