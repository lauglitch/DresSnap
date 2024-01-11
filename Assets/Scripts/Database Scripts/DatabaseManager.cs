using UnityEngine;
using SQLite4Unity3d;

public class DatabaseManager : MonoBehaviour
{
    private string databasePath;
#pragma warning disable CS0436 // El tipo entra en conflicto con un tipo importado
    private static SQLiteConnection connection;
#pragma warning restore CS0436 // El tipo entra en conflicto con un tipo importado

    private void Awake()
    {
        databasePath = Application.persistentDataPath + "/myDatabase.db";         // WARNING: this is just a Testing Database
        //databasePath = Application.persistentDataPath + "/dressnapDatabase.db"; // WARNING: only uncomment in the end to use real Database

        OpenConnection();
    }
    void Start()
    {
        // NOTE: Method calls just to test
        //GetGarmentDBTable();
        //GetGarmentDBTuple(1);
    }

    /*------------------- DATABASE CONNECTION AND SETTING METHODS ------------------- */
    private void OpenConnection()
    {
        connection = new SQLiteConnection(databasePath);
        if (connection != null)
        {
            Logger.Log(LogLevel.Debug, "Connection to database opened successfully in: " + databasePath);

            //connection.DropTable<GarmentDB>(); // NOTE: only uncomment if want to delete table and reset autoincrement

            // Check if database exists in order to create or not new table GarmentDB
            if (TableExists("GarmentDB") == false)
            {
                CreateTable();
                ClearData();
                InsertFirstData();
            } else
            {
                Logger.Log(LogLevel.Debug, "GarmentDB table created on " + databasePath);
            }
        }
        else
        {
            Logger.Log(LogLevel.Error, "Error opening database connection on: " + databasePath);
        }
    }
    private bool TableExists(string tableName)
    {
        string query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
        var result = connection.ExecuteScalar<string>(query);
        return result != null;
    }
    private void CreateTable()
    {
        try
        {
            connection.CreateTable<Garment>();
            Logger.Log(LogLevel.Debug, "GarmentDB table created succcesfully.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error creating GarmentDB table: " + e.Message);
        }
    }
    //TODO: Delete later
    private void InsertFirstData()
    {
        try
        {
            connection.Insert(new Garment { Name = "Camiseta Rosa", Type = "Top", ImagePath="undefined"});
            connection.Insert(new Garment { Name = "Pantalón vaquero Zara", Type = "Bottom", ImagePath = "undefined" });
            Logger.Log(LogLevel.Debug, "First data inserted successfully.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error inserting data: " + e.Message);
        }
    }
    private void ClearData()
    {
        try
        {
            connection.DeleteAll<Garment>();
            Logger.Log(LogLevel.Debug, "Data deleted successfully.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error deleting data: " + e.Message);
        }
    }

    /*-------------------------------- QUERY METHODS -------------------------------- */
    public static void InsertGarmentDB(Garment newGarment)
    {
        try
        {
            connection.Insert(newGarment);
            Logger.Log(LogLevel.Debug, "Data successfully inserted into GarmentDB.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error inserting data into GarmentDB: " + e.Message);
        }
    }
    public void DeleteGarmentDB(int garmentID)
    {
        try
        {
            connection.Delete<Garment>(garmentID);
            Logger.Log(LogLevel.Debug, "Garment with id= " + garmentID + " successfully deleted from GarmentDB.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error deleting garment with id= " + garmentID + " from garmentID: " + e.Message);
   
        }
    }
    public void UpdateGarmentDB(Garment existingGarment)
    {
        try
        {
            connection.Update(existingGarment);
            Logger.Log(LogLevel.Debug, "Garment with id= " + existingGarment.GarmentID + " successfully updated in GarmentDB.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error updating outfit with id= " + existingGarment.GarmentID + " in GarmentDB: " + e.Message);
        }
    }
    public void InsertOutfitDB(Outfit newOutfit)
    {
        try
        {
            connection.Insert(newOutfit);
            Logger.Log(LogLevel.Debug, "Data successfully inserted into OutfitDB.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error inserting data into OutfitDB: " + e.Message);
        }
    }
    public void DeleteOutfitDB(int outfitID)
    {
        try
        {
            connection.Delete<Outfit>(outfitID);
            Logger.Log(LogLevel.Debug, "Outfit with id= " + outfitID + " successfully deleted from OutfitDB.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error deleting outfit with id= " + outfitID + " from OutfitDB: " + e.Message);
        }
    }
    public void UpdateOutfitDB(Outfit existingOutfit)
    {
        try
        {
            connection.Update(existingOutfit);
            Logger.Log(LogLevel.Debug, "Outfit with id= " + existingOutfit.OutfitID + " successfully updated in OutfitDB.");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error updating outfit with id= " + existingOutfit.OutfitID + " in OutfitDB: " + e.Message);
        }
    }
    private void GetGarmentDBTable()
    {
        try
        {
            var garmentDB = connection.Table<Garment>();

            foreach (var garment in garmentDB)
            {
                Debug.Log("ID: " + garment.GarmentID + ", Name: " + garment.Name + ", Type: " + garment.Type + ", ImagePath: " + garment.ImagePath);
            }
            Logger.Log(LogLevel.Debug, "Number of tuples: " + connection.Table<Garment>().Count());
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error getting GarmentDB table data: " + e.Message);
        }
    }
    private void GetOutfitDBTable()
    {
        try
        {
            var outfitDB = connection.Table<Outfit>();

            foreach (var outfit in outfitDB)
            {
                Debug.Log("ID: " + outfit.OutfitID + ", TopID: " + outfit.TopID + ", BottomID: " + outfit.BottomID + ", ShoesID: " + outfit.ShoesID);
            }
            Logger.Log(LogLevel.Debug, "Number of tuples: " + connection.Table<Garment>().Count());
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error getting OutfitDB table data: " + e.Message);
        }
    }
    private void GetGarmentDBTuple(int garmentID)
    {
        try
        {
            var garmentDB = connection.Table<Garment>();

            Garment garmentFounded = garmentDB.Where(p => p.GarmentID == garmentID).FirstOrDefault();
            Debug.Log($"Garment founded with ID: {garmentFounded.GarmentID}, Name: {garmentFounded.Name}, Type: {garmentFounded.Type} , ImagePath: {garmentFounded.ImagePath}");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error getting GarmentDB tuple with id: " + e.Message);
        }
    }
    private void GetOutfitDBTuple(int outfitID)
    {
        try
        {
            var outfitDB = connection.Table<Outfit>();

            Outfit outfitFounded = outfitDB.Where(p => p.OutfitID == outfitID).FirstOrDefault();
            Debug.Log($"Outfit founded with ID: {outfitFounded.OutfitID}, TopID: {outfitFounded.TopID}, TyBottomIDpe: {outfitFounded.BottomID} , ShoesID: {outfitFounded.ShoesID}");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error getting OutfitDB table data: " + e.Message);
        }
    }
}

/*--------------------------------- DATABASE TABLES --------------------------------- */
// Database table that contains every single Garment
public class Garment
{
    [PrimaryKey, AutoIncrement]
    public int GarmentID { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // Types: 'Top', 'Bottom' and 'Shoes'
    public string ImagePath { get; set; }

    public void InsertGarmentDB (Garment newGarment)
    {
        DatabaseManager.InsertGarmentDB(newGarment);
    }
}

// Database table that relates 3 garments in a unique outfit
public class Outfit
{
    [PrimaryKey, AutoIncrement]
    public int OutfitID { get; set; }
    public int TopID { get; set; }
    public int BottomID { get; set; }
    public int ShoesID { get; set; }

}
