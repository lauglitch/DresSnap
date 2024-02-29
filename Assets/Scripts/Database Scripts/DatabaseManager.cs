using UnityEngine;
using SQLite4Unity3d;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

public class DatabaseManager : MonoBehaviour
{
    private static string databasePath;
    private static SQLiteConnection connection;

    public static Queue<Outfit> outfitsQueue = new Queue<Outfit>();
    public static Dictionary<int, Garment> garmentsDictionary = new Dictionary<int, Garment>();

    private const string GARMENT_TABLE_NAME = "Garment";
    private const string OUTFIT_TABLE_NAME = "Outfit";
    private const string ALL = "All";
    public const int NO_GARMENT_ID = -1;
    public const string UNDEFINED = "undefined";
    public const string NO_IMAGE_PATH = "undefined";

    void Awake()
    {
        InitializeDatabase("myDatabase.db");
        // InitializeDatabase("dressnapDatabase.db"); // TODO: only uncomment in the end to use real Database

        StartTestingData(); // TODO: Delete in the future, this is just for testing data
    }

    private void InitializeDatabase(string dbName)
    {
        string path = Application.persistentDataPath + "/" + dbName;

        // TODO: only uncomment in the end to use real Database
        //databasePath = Application.persistentDataPath + "/dressnapDatabase.db"; 

        connection = new SQLiteConnection(path);

        if (connection != null)
        {
            Logger.Log(LogLevel.Success, $"Connection to database opened successfully in: {path}");
        }
        else
        {
            Logger.Log(LogLevel.Error, $"Error opening database connection on: {path}");
        }
    }

    void Start()
    {
        // Load all outftis from database when the app is starting
        DatabaseManager.LoadOutfitsOnLocalDS();
        DatabaseManager.LoadGarmentsOnLocalDS();
    }

    private void StartTestingData()
    {
        Task.Run(async () =>
        {
            await OpenConnectionAsync();
            await ClearTablesAsync(ALL);
            await CheckTablesExistOrCreate();

            await InsertFirstDataAsync();

            //await PrintAllGarmentsAsync();
            //await PrintAllOutfitsAsync();

            await LoadOutfitsOnLocalDS();
            await LoadGarmentsOnLocalDS();

            //await PrintOutfitQueue();
            //await PrintGarmentDictionary();
        });
    }

    /*---------------------------------- INITIALIZE DATABASE ----------------------------------*/
    private async Task OpenConnectionAsync()
    {
        if (connection == null)
        {
            Logger.Log(LogLevel.Error, "Connection is not initialized.");
            return;
        }

        await Task.Run(() =>
        {
            if (connection != null)
            {
                Logger.Log(LogLevel.Success, $"Connection to database opened successfully in: {databasePath}");
            }
            else
            {
                Logger.Log(LogLevel.Error, $"Error opening database connection on: {databasePath}");
            }
        });
    }
    
    private async Task CheckTablesExistOrCreate()
    {
        await Task.Run(() =>
        {
            if (!TableExists(GARMENT_TABLE_NAME))
            {
                connection.CreateTable<Garment>();
                Logger.Log(LogLevel.Success, $"Garment table created on: {databasePath}");
            }
            else
            {
                Logger.Log(LogLevel.Success, $"Garment table already exists on: {databasePath}");
            }

            if (!TableExists(OUTFIT_TABLE_NAME))
            {
                connection.CreateTable<Outfit>();
                Logger.Log(LogLevel.Success, $"Outfit table created on: {databasePath}");
            }
            else
            {
                Logger.Log(LogLevel.Success, $"Outfit table already exists on: {databasePath}");
            }
        });
    }

    private bool TableExists(string tableName)
    {
        if (connection == null)
        {
            Logger.Log(LogLevel.Error, "Connection is not initialized.");
            return false;
        }

        string query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
        var result = connection.ExecuteScalar<string>(query);
        return result != null;
    }

    private async Task InsertFirstDataAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                connection.Insert(new Garment { Name = "Camiseta Kiss", Type = "Top", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/camisetakiss.PNG" });
                connection.Insert(new Garment { Name = "Camisa azul Zara", Type = "Top", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/camisetaazulzara.PNG" });
                connection.Insert(new Garment { Name = "Camiseta blanca", Type = "Top", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/camisetablanca.PNG" });
                connection.Insert(new Garment { Name = "Camiseta Iron Maiden", Type = "Top", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/camisetaironmaiden.PNG" });
                connection.Insert(new Garment { Name = "Camiseta Sailor Moon", Type = "Top", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/camisetasailormoon.PNG" });
                connection.Insert(new Garment { Name = "Jersey morado cisne", Type = "Top", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/jerseymoradocisne.PNG" });
                connection.Insert(new Garment { Name = "Jersey negro", Type = "Top", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/jerseynegro.PNG" });

                connection.Insert(new Garment { Name = "Pantalones encerados", Type = "Bottom", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/pantalonesencerados.PNG" });
                connection.Insert(new Garment { Name = "Vaquero azul", Type = "Bottom", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/vaquerosazules.PNG" });
                connection.Insert(new Garment { Name = "Vaquero negro", Type = "Bottom", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/vaquerosnegros.PNG" });
                connection.Insert(new Garment { Name = "Falda negra", Type = "Bottom", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/faldanegra.PNG" });
                connection.Insert(new Garment { Name = "Pantalones beige pana", Type = "Bottom", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/pantalonesbeigepana.PNG" });
                connection.Insert(new Garment { Name = "Pantalones negros campana", Type = "Bottom", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/pantalonesnegroscampana.PNG" });

                connection.Insert(new Garment { Name = "Botas militares", Type = "Shoes", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/botasmilitares.PNG" });
                connection.Insert(new Garment { Name = "Adidas blancas", Type = "Shoes", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/adidasblancas.PNG" });
                connection.Insert(new Garment { Name = "Converse blancas", Type = "Shoes", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/converseblancas.PNG" });
                connection.Insert(new Garment { Name = "Vans negras", Type = "Shoes", ImagePath = "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/vansnegras.PNG" });

                connection.Insert(new Outfit { TopID = 1, BottomID = 8, ShoesID = NO_GARMENT_ID });
                connection.Insert(new Outfit { TopID = 2, BottomID = 12, ShoesID = 16 });
                connection.Insert(new Outfit { TopID = 3, BottomID = 9, ShoesID = 17 });

                connection.Insert(new Outfit { TopID = 4, BottomID = 11, ShoesID = 14 });
                connection.Insert(new Outfit { TopID = 5, BottomID = NO_GARMENT_ID, ShoesID = 16 });
                connection.Insert(new Outfit { TopID = 6, BottomID = 10, ShoesID = 17 });

                connection.Insert(new Outfit { TopID = 6, BottomID = 8, ShoesID = 14 });
                connection.Insert(new Outfit { TopID = 7, BottomID = NO_GARMENT_ID, ShoesID = NO_GARMENT_ID });
                //connection.Insert(new Outfit { TopID = 7, BottomID = 12, ShoesID = 15 });
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, "Error inserting data: " + e.Message);
            }
        });
    }

    private async Task ClearDataAsync(string tableName)
    {
        await Task.Run(() =>
        {
            try
            {
                Logger.Log(LogLevel.Success, "Data deleted successfully.");

                if (tableName.Equals(GARMENT_TABLE_NAME))
                {
                    connection.DeleteAll<Garment>();
                    Logger.Log(LogLevel.Success, "Garment table data deleted successfully.");
                }
                else if (tableName.Equals(OUTFIT_TABLE_NAME))
                {
                    connection.DeleteAll<Outfit>();
                    Logger.Log(LogLevel.Success, "Outfit table data deleted successfully.");
                }
                else if (tableName.Equals("All"))
                {
                    connection.DeleteAll<Outfit>();
                    connection.DeleteAll<Garment>();
                    Logger.Log(LogLevel.Success, "All tables data deleted successfully.");
                }
                else
                {
                    Logger.Log(LogLevel.Error, "Invalid parameter in ClearTablesAsync()");
                }
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error deleting data: {e.Message}");
            }
        });
    }

    private async Task ClearTablesAsync(string tableName)
    {
        await Task.Run(() =>
        {
            try
            {
                if (tableName.Equals(GARMENT_TABLE_NAME))
                {
                    if (TableExists(GARMENT_TABLE_NAME))
                    {
                        connection.DropTable<Garment>();
                        Logger.Log(LogLevel.Success, "Garment table dropped successfully.");
                    }
                    else
                    {
                        Logger.Log(LogLevel.Warning, "Garment table was not dropped because it doesn't exist.");
                    }
                }
                else if (tableName.Equals(OUTFIT_TABLE_NAME))
                {
                    if (TableExists(OUTFIT_TABLE_NAME))
                    {
                        connection.DropTable<Outfit>();
                        Logger.Log(LogLevel.Success, "Outfit table dropped successfully.");
                    }
                    else
                    {
                        Logger.Log(LogLevel.Warning, "Outfit table was not dropped because it doesn't exist.");
                    }
                }
                else if (tableName.Equals("All"))
                {
                    // Recursive calls
                    ClearTablesAsync(GARMENT_TABLE_NAME).Wait();
                    ClearTablesAsync(OUTFIT_TABLE_NAME).Wait();
                    //Logger.Log(LogLevel.Success, "All tables dropped successfully.");
                }
                else
                {
                    Logger.Log(LogLevel.Error, "Invalid parameter in ClearTablesAsync()");
                }
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error deleting data: {e.Message}");
            }
        });
    }

    /*---------------------------------- I/O DATABASE -------------------------------------*/
    public static async Task<int> InsertGarmentAsync(Garment newGarment)
    {
        int insertedID = -1; // Default value if insert fails

        await Task.Run(() =>
        {
            try
            {
                connection.Insert(newGarment);
                insertedID = newGarment.GarmentID;
                Logger.Log(LogLevel.Success, $"Garment with id={insertedID} successfully inserted into Garment.");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error inserting data into Garment: {e.Message}");
            }
        });

        return insertedID;
    }
    
    public static async Task DeleteGarmentAsync(int garmentID)
    {
        await Task.Run(() =>
        {
            try
            {
                connection.Delete<Garment>(garmentID);
                Logger.Log(LogLevel.Success, $"Garment with id= {garmentID} successfully deleted from Garment.");

            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error deleting garment with id= {garmentID} from garmentID: {e.Message}");
            }
        });
    }

    public static async Task UpdateGarmentAsync(Garment existingGarment)
    {
        await Task.Run(() =>
        {
            try
            {
                connection.Update(existingGarment);
                Logger.Log(LogLevel.Success, $"Garment with id= {existingGarment.GarmentID} successfully updated in Garment.");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error updating outfit with id= {existingGarment.GarmentID} in Garment: {e.Message}");
            }
        });
    }

    public static async Task<int> InsertOutfitAsync(Outfit newOutfit)
    {
        int insertedID = -1; // Default value if insert fails

        await Task.Run(() =>
        {
            try
            {
                connection.Insert(newOutfit);
                insertedID = newOutfit.OutfitID;
                Logger.Log(LogLevel.Success, "Outfit with id=" + insertedID + " successfully inserted into Garment.");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error inserting data into Outfit: {e.Message}");
            }
        });

        return insertedID;
    }

    public static async Task DeleteOutfitAsync(int outfitID)
    {
        await Task.Run(() =>
        {
            try
            {
                connection.Delete<Outfit>(outfitID);
                Logger.Log(LogLevel.Success, $"Outfit with id= {outfitID} successfully deleted from Outfit.");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error deleting outfit with id= {outfitID} from Outfit: {e.Message}");
            }
        });
    }

    public static async Task UpdateOutfitAsync(Outfit existingOutfit)
    {
        await Task.Run(() =>
        {
            try
            {
                connection.Update(existingOutfit);
                Logger.Log(LogLevel.Success, $"Outfit with id= {existingOutfit.OutfitID} successfully updated in Outfit.");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error updating outfit with id= {existingOutfit.OutfitID} in Outfit: {e.Message}");
            }
        });
    }

    public static async Task<Queue<Outfit>> GetAllOutfitsAsync()
    {
        try
        {
            return await Task.Run(() =>
            {
                var outfits = connection.Table<Outfit>().ToList();

                if (outfits == null || outfits.Count == 0)
                {
                    Logger.Log(LogLevel.Warning, "There are no outfits in the database.");
                    return new Queue<Outfit>();
                }

                return new Queue<Outfit>(outfits);
            });
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, "Error getting all outfits from database: " + e.Message);
            return new Queue<Outfit>();
        }
    }
    
    public static async Task<List<Garment>> GetAllGarmentsAsync()
    {
        try
        {
            return await Task.Run(() =>
            {
                var garments = connection.Table<Garment>().ToList();
                return garments;
            });
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, $"Error getting all garments from database: {e.Message}");
            return new List<Garment>();
        }
    }

    public static async Task<Garment> GetGarmentAsync(int garmentID)
    {
        try
        {
            return await Task.Run(() =>
            {
                var garment = connection.Table<Garment>().FirstOrDefault(p => p.GarmentID == garmentID);
                return garment;
            });
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, $"Error getting Garment tuple with id: {e.Message}");
            return null;
        }
    }

    public static async Task GetOutfitAsync(int outfitID)
    {
        await Task.Run(() =>
        {
            try
            {
                var Outfit = connection.Table<Outfit>();

                Outfit outfitFounded = Outfit.Where(p => p.OutfitID == outfitID).FirstOrDefault();
                Logger.Log(LogLevel.Info, $"Outfit founded with ID: {outfitFounded.OutfitID}, TopID: {outfitFounded.TopID}, BottomID: {outfitFounded.BottomID}, ShoesID: {outfitFounded.ShoesID}");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error getting Outfit table data: {e.Message}");
            }
        });
    }


    /*---------------------------------- UI METHODS -------------------------------------*/
    public static async Task UpdateLocalDataStructures()
    {
        await Task.Run(() =>
        {
            try
            {
                LoadOutfitsOnLocalDS();
                LoadGarmentsOnLocalDS();

                Logger.Log(LogLevel.Info, "Local data updated succesfully");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error updating local data structures: {e.Message}");
            }
        });
    }

    public static async Task LoadOutfitsOnLocalDS()
    {
        await Task.Run(async () =>
        {
            try
            {
                var outfits = await GetAllOutfitsAsync();
                foreach (var outfit in outfits)
                {
                    outfitsQueue.Enqueue(outfit);
                }

                Logger.Log(LogLevel.Info, "Local outfits updated succesfully on queue");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error updating local outfits queue: {e.Message}");
            }
        });
    }

    public static async Task LoadGarmentsOnLocalDS ()
    {
        await Task.Run(async () =>
        {
            try
            {
                var garments = await GetAllGarmentsAsync();
                foreach (var garment in garments)
                {
                    garmentsDictionary.Add(garment.GarmentID, garment);
                }

                Logger.Log(LogLevel.Info, "Local garments updated succesfully on dictionary");
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error updating local garments dictionary: {e.Message}");
            }
            
        });
    }

    public static Outfit PickRandomLocalOutfit()
    {
        try
        {
            if (outfitsQueue.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, outfitsQueue.Count);
                return outfitsQueue.ElementAt(randomIndex);
            }
            else
            {
                Logger.Log(LogLevel.Warning, "No outfits on database.");
                return null;
            }
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, $"Error getting random outfit from queue 'outfitsQueue': {e.Message}");
            return null;
        }
    }

    /*---------------------------------- DEBUG METHODS ----------------------------------*/
    public static async Task PrintAllOutfitsOnDB()
    {
        try
        {
            Queue<Outfit> outfits = await GetAllOutfitsAsync();

            foreach (var outfit in outfits)
            {
                Logger.Log(LogLevel.Success, $"Outfit ID: {outfit.OutfitID}, TopID: {outfit.TopID}, BottomID: {outfit.BottomID}, ShoesID: {outfit.ShoesID}");
            }
        }
        catch (SQLiteException ex)
        {
            Logger.Log(LogLevel.Error, $"Error printing outfits: {ex.Message}");
        }
    }

    public static async Task PrintAllGarmentsOnDB()
    {
        try
        {
            var garments = await GetAllGarmentsAsync();

            foreach (var garment in garments)
            {
                Logger.Log(LogLevel.Success, $"Garment ID: {garment.GarmentID}, Name: {garment.Name}, Type: {garment.Type}, ImagePath: {garment.ImagePath}");
            }
        }
        catch (SQLiteException ex)
        {
            Logger.Log(LogLevel.Error, $"Error printing garments: {ex.Message}");
        }
    }

    public static Task PrintAllOutfitsOnLocal()
    {
        try
        {
            foreach (var outfit in outfitsQueue)
            {
                Logger.Log(LogLevel.Success, $"Outfit ID: {outfit.OutfitID}, TopID: {outfit.TopID}, BottomID: {outfit.BottomID}, ShoesID: {outfit.ShoesID}");
            }
        }
        catch (SQLiteException ex)
        {
            Logger.Log(LogLevel.Error, $"Error printing outfits: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    public static Task PrintAllGarmentsOnLocal()
    {
        try
        {
            foreach (var garment in garmentsDictionary)
            {
                Logger.Log(LogLevel.Success, $"Garment ID: {garment.Key}, Name: {garment.Value.GarmentID}, Type: {garment.Value.Type}, ImagePath: {garment.Value.ImagePath}");
            }
        }
        catch (SQLiteException ex)
        {
            Logger.Log(LogLevel.Error, $"Error printing garments: {ex.Message}");
        }

        return Task.CompletedTask;
    }

    /*---------------------------------- VALIDATION METHODS -----------------------------*/
    public static async Task<bool> ValidateOutfitInfoAsync(List<Garment> garmentList)
    {
        if (garmentList == null || garmentList.Count == 0) 
        {
            Logger.Log(LogLevel.Error, "An outfit must have at least one garment.");
            return false;
        }

        // Validate each garment in the list
        foreach (var garment in garmentList)
        {
            if (!await ValidateGarmentInfoAsync(garment))
            {
                Logger.Log(LogLevel.Error, "Invalid garment in the outfit.");
                return false;
            }
        }

        // Check for duplicate ImagePath and Name within the list
        if (HasDuplicateValues(garmentList.Select(g => g.ImagePath)))
        {
            Logger.Log(LogLevel.Error, "Outfit contains garments with duplicate ImagePath.");
            return false;
        }

        if (HasDuplicateValues(garmentList.Select(g => g.Name)))
        {
            Logger.Log(LogLevel.Error, "Outfit contains garments with duplicate Name.");
            return false;
        }

        return true;
    }

    private static async Task<bool> ValidateGarmentInfoAsync(Garment garment)
    {
        // Check if Garment.Name is NOT NULL or empty
        if (string.IsNullOrEmpty(garment.Name))
        {
            Logger.Log(LogLevel.Error, "Garment name cannot be null or empty.");
            return false;
        }

        // Check if Garment.Name is UNIQUE
        if (!await NameIsUniqueAsync(garment.Name, garment.GarmentID))
        {
            Logger.Log(LogLevel.Error, "Garment name must be unique.");
            return false;
        }

        // Check if Garment.Type is NOT NULL
        if (string.IsNullOrEmpty(garment.Type) || !ValidGarmentType(garment.Type))
        {
            Logger.Log(LogLevel.Error, $"Invalid garment type. Type must be 'Top', 'Bottom', or 'Shoes'. id= {garment.GarmentID}");
            return false;
        }

        // Check if Garment.ImagePath is UNIQUE
        bool isImagePathUnique = await IsImagePathUniqueAsync(garment.ImagePath, garment.GarmentID);
        if (!isImagePathUnique)
        {
            Logger.Log(LogLevel.Error, " Image path is not unique. " + "id= " + garment.GarmentID);
            return false;
        }

        return true;
    }

    // Check if there are duplicate values in a sequence (of anything)
    private static bool HasDuplicateValues<T>(IEnumerable<T> values)
    {
        var uniqueValues = new HashSet<T>();
        foreach (var value in values)
        {
            if (!uniqueValues.Add(value))
            {
                // Value already exists, indicating a duplicate
                return true;
            }
        }
        return false;
    }

    private static bool ValidGarmentType(string type)
    {
        // Valid garment types
        string[] validTypes = { "Top", "Bottom", "Shoes" };
        return validTypes.Contains(type);
    }

    public static bool GarmentIsEmpty(Garment garment)
    {
        // Si el nombre y la imagen están ambos vacíos, consideramos que el Garment está vacío.
        return string.IsNullOrEmpty(garment.Name) && string.IsNullOrEmpty(garment.ImagePath);
    }

    private static async Task<bool> NameIsUniqueAsync(string name, int currentGarmentID = -1)
    {
        try
        {
            var allGarments = await GetAllGarmentsAsync();

            // Check if any garment has the same name
            bool isNotUnique = allGarments.Any(g => g.Name == name && g.GarmentID != currentGarmentID);

            // If not unique, return false; otherwise, return true
            return !isNotUnique;
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error checking uniqueness of garment name: {ex.Message}");
            return false;
        }
    }

    private static async Task<bool> IsImagePathUniqueAsync(string imagePath, int currentGarmentID = -1)
    {
        try
        {
            var allGarments = await GetAllGarmentsAsync();

            // Check if any garment has the same image path
            bool isNotUnique = allGarments.Any(g => g.ImagePath == imagePath && g.GarmentID != currentGarmentID);

            // If not unique, return false; otherwise, return true
            return !isNotUnique;
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error checking uniqueness of image path: {ex.Message}");
            return false;
        }
    }
}

/*--------------------------------- DATABASE TABLES --------------------------------- */
// Database table that contains every single Garment
[Table("Garment")]
public class Garment
{
    [PrimaryKey, AutoIncrement]
    public int GarmentID { get; set; }
    [NotNull]
    public string Name { get; set; }
    [NotNull]
    public string Type { get; set; } // Types: 'Top', 'Bottom' and 'Shoes'
    public string ImagePath { get; set; }

    public void InsertGarment(Garment newGarment)
    {
        Task.Run(() =>
        {
            try
            {
                DatabaseManager.InsertGarmentAsync(newGarment).Wait();
            }
            catch (SQLiteException e)
            {
                Logger.Log(LogLevel.Error, $"Error inserting garment into database: {e.Message}");
            }
        });
    }
}

// Database table that relates 3 garments in a unique outfit
[Table("Outfit")]
public class Outfit
{
    [PrimaryKey, AutoIncrement]
    public int OutfitID { get; set; }
    public int TopID { get; set; }
    public int BottomID { get; set; }
    public int ShoesID { get; set; }
}
