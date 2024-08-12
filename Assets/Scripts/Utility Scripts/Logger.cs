using System;
using System.Collections.Generic;
using UnityEngine;

public enum LogLevel
{
    Info,           // Mensajes informativos generales
    Warning,        // Mensajes de advertencia
    Success,        // Mensajes de éxito (puede tratarse como Info en cuanto al color)
    Error,          // Mensajes de error
    Test,           // Mensajes de prueba (solo para desarrollo)
    DeepTest,       // Mensajes de prueba detallada (solo para desarrollo)
    Instances,      // Mensajes para info sobre creación e instanciación de objetos (solo para desarrollo)
    DBConnection,   // Mensajes para info sobre conexión a la BD (solo para desarrollo)
    DBCRUD          // Mensajes para proceso CRUD de la BD (solo para desarrollo)
}

public static class Logger
{
    // Especifica manualmente los niveles de log que deseas mostrar
    public static List<LogLevel> onlyShow = new List<LogLevel> {
        //LogLevel.Info,
        //LogLevel.Warning,
        //LogLevel.Success,
        LogLevel.Error,

        //LogLevel.Test,
        //LogLevel.DeepTest,

        //LogLevel.DBConnection,
        LogLevel.DBCRUD,
    };

    public static void Log(LogLevel level, string message)
    {
        if (onlyShow.Contains(level))
        {
            switch (level)
            {
                case LogLevel.Warning:
                    Debug.LogWarning(level + " - " + message);
                    break;
                case LogLevel.Error:
                    Debug.LogError(level + " - " + message);
                    break;
                default:
                    Debug.Log(level + " - " + message);
                    break;
            }
        }
    }

    public static void SeveralLogs(LogLevel level, List<String> messages)
    {
        if (onlyShow.Contains(level))
        {
            String allMessages = "";
            foreach (String message in messages)
            {
                allMessages += "\n" + message;
            }

            // Usa el método apropiado para el nivel de log
            Log(level, allMessages);
        }
    }

    public static void GameObjectString(LogLevel level, GameObject gameobject)
    {
        if (onlyShow.Contains(level))
        {
            Log(level, gameobject.name);
        }
    }

    public static string GarmentString(Garment garment)
    {
        return "Garment ID=" + garment.GarmentID + ", Name=" + garment.Name + ", Type=" + garment.Type + ", ImagePath=" + garment.ImagePath;
    }

    public static string OutfitString(Outfit outfit)
    {
        return "Outfit ID=" + outfit.OutfitID + " -> TopID=" + outfit.TopID + ", BottomID=" + outfit.BottomID + ", ShoesID=" + outfit.ShoesID;
    }
}
