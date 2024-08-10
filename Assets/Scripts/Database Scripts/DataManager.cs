using System.Collections.Generic;
using UnityEngine;

public class DataManager: MonoBehaviour
{
    // Estructuras de datos compartidas
    public List<Garment> garmentsList { get; set; }
    public Queue<Outfit> outfitsQueue { get; private set; }

    private void Awake()
    {
        Logger.Log(LogLevel.DeepTest, "DataManager Awake() method called");
    }
    // Constructor privado para evitar la instanciación directa
    private DataManager()
    {
        // Inicialización de las estructuras de datos
        garmentsList = new List<Garment>();
        outfitsQueue = new Queue<Outfit>();
    }

    // Métodos para actualizar las estructuras de datos
    public void UpdateGarments(List<Garment> updatedGarments)
    {
        garmentsList = updatedGarments;
    }

    public void UpdateOutfits(Queue<Outfit> updatedOutfits)
    {
        outfitsQueue = updatedOutfits;
    }

    public void PrintOutfitsQueue()
    {
        foreach (var o in outfitsQueue)
        {
            Logger.Log(LogLevel.Info, $"Outfit ID: {o.OutfitID}, TopID: {o.TopID}, BottomID: {o.BottomID}, ShoesID: {o.ShoesID}");
        }

    }

    public void PrintGarmentsList()
    {
        foreach (var g in garmentsList)
        {
            Logger.Log(LogLevel.Info, $"Garment ID: {g.GarmentID}, Name: {g.Name}, Image Path: {g.ImagePath}");
        }
    }
}
