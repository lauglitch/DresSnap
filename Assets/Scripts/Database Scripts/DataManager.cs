using System.Collections.Generic;
using UnityEngine;

public class DataManager: MonoBehaviour
{
    public static DataManager instance { get; set; }

    // Estructuras de datos compartidas
    public Queue<Outfit> outfitsQueue { get; set; }
    public List<Garment> garmentsList { get; set; }

    public DataManager()
    {
        outfitsQueue = new Queue<Outfit>();
        garmentsList = new List<Garment>();
    }

    private void Awake()
    {
        Logger.Log(LogLevel.Instances, "CoreManager Awake() method called");

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        Logger.Log(LogLevel.Instances, "DataManager Start() method called");
    }

    // Constructor privado para evitar la instanciación directa

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
