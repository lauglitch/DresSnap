using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Class that manages the backup of the local Outfit and Garment data structures and their respective 
/// real tables in the Database
/// </summary>
public class BackupDataManager: MonoBehaviour
{
    private DataManager dataManager;
    public  List<Garment> backupGarmentsList { get; set; }
    public Queue<Outfit> backupOutfitsQueue { get; set; }

    void Awake()
    {
        Logger.Log(LogLevel.DeepTest, "BackupDataManager Awake() method called");
    }

    public void SetDataManager(DataManager dm)
    {
        dataManager = dm;
    }

    public void BackupData()
    {
        try
        {
            BackupLocalData();
            BackupDatabaseData();
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error backing up data: {ex.Message}");
        }
    }

    public async Task InitializeBackupGarmentsListAsync()
    {
        // Get the list of garments from the database asynchronously
        backupGarmentsList = await DatabaseManager.GetAllGarmentsAsync();
        backupOutfitsQueue = await DatabaseManager.GetAllOutfitsAsync();
    }

    private void BackupLocalData()
    {
        // Support local data structures (garmentsList and outfitsQueue).
        backupGarmentsList = new List<Garment>(dataManager.garmentsList);
        backupOutfitsQueue = new Queue<Outfit>(dataManager.outfitsQueue);
    }

    private void BackupDatabaseData()
    {
        // Respaldar los datos relevantes desde la base de datos SQL.
    }

    public void RollbackData()
    {
        try
        {
            RollbackLocalData();
            RollbackDatabaseData();
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error rolling back data: {ex.Message}");
        }
    }

    private void RollbackLocalData()
    {
        try
        {
            // Clean local data structures
            dataManager.garmentsList.Clear();
            dataManager.outfitsQueue.Clear();

            // Restore data from local backups.
            if (backupGarmentsList != null)
            {
                foreach (var garment in backupGarmentsList)
                {
                    dataManager.garmentsList[garment.GarmentID] = garment;
                }
            }

            if (backupOutfitsQueue != null)
            {
                dataManager.UpdateOutfits(new Queue<Outfit>(backupOutfitsQueue));
            }
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error rolling back local data: {ex.Message}");
        }
    }

    private void RollbackDatabaseData()
    {
        try
        {
            // Implementar la lógica para revertir los datos en la base de datos SQL
            // utilizando los respaldos de la base de datos.
            // Esto podría implicar eliminar registros que se hayan insertado durante el respaldo.

            // Puedes usar los valores de backup para identificar y eliminar los registros
            // insertados durante el respaldo.
            // Aquí tendrías que implementar la lógica específica para revertir los cambios
            // realizados en la base de datos durante la operación de respaldo.
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error rolling back database data: {ex.Message}");
        }
    }
}
