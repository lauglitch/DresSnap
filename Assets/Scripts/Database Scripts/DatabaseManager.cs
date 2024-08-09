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

    private static DataManager dataManager;

    //public static Queue<Outfit> outfitsQueue = new Queue<Outfit>();   // TODO: delete
    //public static List<Garment> garmentsList = new List<Garment>();   // TODO: delete

    public static bool[] areGarmentsGoingToBeSaved = new bool[3];

    void Awake()
    {
    

    }

    void Start()
    {
        dataManager = CoreManager.Instance.GetDataManager();

        if (dataManager == null)
        {
            Debug.LogError("Could not get DataManager from CoreManager.Instance");
        }

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
            Logger.Log(LogLevel.DBConnection, $"Connection to database opened successfully in: {path}");
        }
        else
        {
            Logger.Log(LogLevel.Error, $"Error opening database connection on: {path}");
        }
    }

    public void SetDataManager(DataManager dm)
    {
        dataManager = dm;
    }

    private void StartTestingData()
    {
        Task.Run(async () =>
        {
            await OpenConnectionAsync();
            await ClearTablesAsync(Constants.ALL);
            await CheckTablesExistOrCreate();

            await InsertFirstDataAsync();

            // Load all outftis from database when the app is starting
            await LoadOutfitsOnLocalDS();
            await LoadGarmentsOnLocalDS();
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
                Logger.Log(LogLevel.DBConnection, $"Connection to database opened successfully in: {databasePath}");
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
            if (!TableExists(Constants.GARMENT_TABLE_NAME))
            {
                connection.CreateTable<Garment>();
                Logger.Log(LogLevel.DeepTest, $"Garment table created on: {databasePath}");
            }
            else
            {
                Logger.Log(LogLevel.DeepTest, $"Garment table already exists on: {databasePath}");
            }

            if (!TableExists(Constants.OUTFIT_TABLE_NAME))
            {
                connection.CreateTable<Outfit>();
                Logger.Log(LogLevel.DeepTest, $"Outfit table created on: {databasePath}");
            }
            else
            {
                Logger.Log(LogLevel.DeepTest, $"Outfit table already exists on: {databasePath}");
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

                connection.Insert(new Outfit { TopID = 1, BottomID = 8, ShoesID = 14 });
                connection.Insert(new Outfit { TopID = 2, BottomID = 12, ShoesID = 16 });
                connection.Insert(new Outfit { TopID = 3, BottomID = 9, ShoesID = 17 });

                connection.Insert(new Outfit { TopID = 4, BottomID = 11, ShoesID = 14 });
                connection.Insert(new Outfit { TopID = 5, BottomID = 13, ShoesID = 16 });
                connection.Insert(new Outfit { TopID = 6, BottomID = 10, ShoesID = 17 });

                connection.Insert(new Outfit { TopID = 6, BottomID = 8, ShoesID = 17 });
                connection.Insert(new Outfit { TopID = 7, BottomID = 12, ShoesID = 16 });
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

                if (tableName.Equals(Constants.GARMENT_TABLE_NAME))
                {
                    connection.DeleteAll<Garment>();
                    Logger.Log(LogLevel.Success, "Garment table data deleted successfully.");
                }
                else if (tableName.Equals(Constants.OUTFIT_TABLE_NAME))
                {
                    connection.DeleteAll<Outfit>();
                    Logger.Log(LogLevel.Success, "Outfit table data deleted successfully.");
                }
                else if (tableName.Equals(Constants.ALL))
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
                if (tableName.Equals(Constants.GARMENT_TABLE_NAME))
                {
                    if (TableExists(Constants.GARMENT_TABLE_NAME))
                    {
                        connection.DropTable<Garment>();
                        Logger.Log(LogLevel.DeepTest, "Garment table dropped successfully.");
                    }
                    else
                    {
                        Logger.Log(LogLevel.Warning, "Garment table was not dropped because it doesn't exist.");
                    }
                }
                else if (tableName.Equals(Constants.OUTFIT_TABLE_NAME))
                {
                    if (TableExists(Constants.OUTFIT_TABLE_NAME))
                    {
                        connection.DropTable<Outfit>();
                        Logger.Log(LogLevel.DeepTest, "Outfit table dropped successfully.");
                    }
                    else
                    {
                        Logger.Log(LogLevel.Warning, "Outfit table was not dropped because it doesn't exist.");
                    }
                }
                else if (tableName.Equals(Constants.ALL))
                {
                    // Recursive calls
                    ClearTablesAsync(Constants.GARMENT_TABLE_NAME).Wait();
                    ClearTablesAsync(Constants.OUTFIT_TABLE_NAME).Wait();
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
                Logger.Log(LogLevel.Success, "Outfit with id=" + insertedID + " and content [" + newOutfit.TopID + ", "
                    + newOutfit.BottomID + ", " + newOutfit.ShoesID + "] " + "inserted into Garment.");
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

    public static int GetGarmentIDByImagePath(string imagePath)
    {
        // Itera sobre los elementos de la lista
        foreach (var garment in dataManager.garmentsList)
        {
            // Compara el atributo ImagePath del Garment con el proporcionado
            if (garment.ImagePath == imagePath)
            {
                // Si coincide, devuelve el ID del Garment
                return garment.GarmentID;
            }
        }

        // Si no se encuentra un Garment que coincida, devuelve un valor indicativo de no encontrarlo
        return -1;
    }

    public static Garment getGarmentByID(int id)
    {
        // Itera sobre los elementos de la lista
        foreach (Garment garment in dataManager.garmentsList)
        {
            // Compara el atributo ImagePath del Garment con el proporcionado
            if (garment.GarmentID == id)
            {
                // Si coincide, devuelve el ID del Garment
                return garment;
            }
        }

        return null;
    }

    /*---------------------------------- UI METHODS -------------------------------------*/
    public static async Task UpdateLocalDataStructures()
    {
        try
        {
            await Task.Run(async () =>
            {
                await LoadOutfitsOnLocalDS();

                await LoadGarmentsOnLocalDS();
            });

            Logger.Log(LogLevel.DeepTest, "Local data updated successfully");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, $"Error updating local data structures: {e.Message}");
        }
    }

    public static async Task LoadOutfitsOnLocalDS()
    {
        try
        {
            var outfits = await GetAllOutfitsAsync();

            foreach (var outfit in outfits.Reverse())
            {
                dataManager.outfitsQueue.Enqueue(outfit);
            }

            Logger.Log(LogLevel.DeepTest, "Local outfits updated successfully on queue");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, $"Error updating local outfits queue: {e.Message}");
        }
    }

    public static async Task LoadGarmentsOnLocalDS()
    {
        try
        {
            var garments = await GetAllGarmentsAsync();
            foreach (var garment in garments)
            {
                dataManager.garmentsList.Add(garment);
            }

            Logger.Log(LogLevel.DeepTest, "Local garments updated successfully on list");
        }
        catch (SQLiteException e)
        {
            Logger.Log(LogLevel.Error, $"Error updating local garments list: {e.Message}");
        }
    }

    public static Outfit PickRandomLocalOutfit()
    {
        try
        {
            if (dataManager.outfitsQueue.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, dataManager.outfitsQueue.Count);
                return dataManager.outfitsQueue.ElementAt(randomIndex);
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

    /*---------------------------------- VALIDATION METHODS -----------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="garments"></param>
    /// <returns>string= "absentGarment", "sameGarment", "", </returns>
    public static async Task<string> ValidateOutfitInfoAsync(List<Garment> garments)
    {
        // 1º Check if there are exactly 3 Garment
        if (garments.Count == 3)
        {
            // 2º Check if imagePath and name from parameters are different among themselves
            if (AreImagePathsDifferent(garments) && AreNameDifferent(garments))
            {
                // 3º Check if imagePath currently exist on database
                foreach (var garment in garments)
                {
                    bool imagePathExists = await Task.Run(() => isImagePathUnique(garment.ImagePath));

                    // Compare parameter garments with local garmentsList 
                    if (!imagePathExists)
                    {
                        string validationGarmentReturn = ValidateGarmentInfo(garment);
                        if (validationGarmentReturn.Equals(Constants.TRUE)) {
                            if (garment.Type.Equals(Constants.TOP_FIELD))
                                areGarmentsGoingToBeSaved[0] = true;
                            else if (garment.Type.Equals(Constants.BOTTOM_FIELD))
                                areGarmentsGoingToBeSaved[1] = true;
                            else if (garment.Type.Equals(Constants.SHOES_FIELD))
                                areGarmentsGoingToBeSaved[2] = true;
                        } 
                        else if (validationGarmentReturn.Equals(Constants.EXISTING_GARMENT_NAME_ERROR))
                            return Constants.EXISTING_GARMENT_NAME_ERROR;
                        else if (validationGarmentReturn.Equals(Constants.EMPTY_GARMENT_NAME_ERROR))
                            return Constants.EMPTY_GARMENT_NAME_ERROR;
                        else if (validationGarmentReturn.Equals(Constants.NO_GARMENT_TYPE_ERROR))
                            return Constants.NO_GARMENT_TYPE_ERROR;
                    }
                    else
                    {
                        if (garment.Type.Equals(Constants.TOP_FIELD))
                            areGarmentsGoingToBeSaved[0] = false;
                        else if (garment.Type.Equals(Constants.BOTTOM_FIELD))
                            areGarmentsGoingToBeSaved[1] = false;
                        else if (garment.Type.Equals(Constants.SHOES_FIELD))
                            areGarmentsGoingToBeSaved[2] = false;


                        // TODO: Mostrar mensaje de advertencia
                        Logger.Log(LogLevel.Warning, $"Garment '{garment.Name}' already exists in garmentsList and it is not gonna be saved.");
                    }
                }
            } else {
                // TODO: show popup saying "Garments must be different"
                Logger.Log(LogLevel.Error, Constants.SAME_GARMENT_IMAGE_ERROR);
                return Constants.SAME_IMAGE; // SAME_NAME_ERROR
            }
        } else {
            // TODO: show popup saying "An outfit must contain 3 garments"
            Logger.Log(LogLevel.Error, Constants.MISSING_GARMENTS_ERROR);
            return Constants.MISSING_GARMENTS;
        }
      
        return Constants.VALID_OUTFIT;
    }

    // Método que valida la información de una prenda con respecto a las existentes en BD
    private static string ValidateGarmentInfo(Garment garment)
    {
        // Verificar si Garment.Name NO es NULL ni está vacío
        if (string.IsNullOrEmpty(garment.Name))
        {
            Logger.Log(LogLevel.Error, "Garment name cannot be null or empty.");
            return Constants.EMPTY_GARMENT_NAME_ERROR;
        }

        // Verificar si Garment.Name es ÚNICO
        if (!isNameUnique(garment.Name))
        {
            Logger.Log(LogLevel.Error, $"The garment name '{garment.Name}' is already in use.");
            return Constants.EXISTING_GARMENT_NAME_ERROR;
        }

        // Verificar si Garment.Type NO es NULL
        if (string.IsNullOrEmpty(garment.Type) || !isTypeValid(garment.Type))
        {
            Logger.Log(LogLevel.Error, $"Invalid garment type. Type must be 'Top', 'Bottom', or 'Shoes'. id= {garment.GarmentID}");
            return Constants.NO_GARMENT_TYPE_ERROR;
        }

        return Constants.TRUE;
    }

    // Método que verifica si un nombre de imagen ya existe en la base de datos de prendas
    private static bool isImagePathUnique(string imagePath)
    {
        try
        {
            // Verificar si alguna prenda tiene la misma ruta de imagen
            bool alreadyExists = dataManager.garmentsList.Any(g =>
            {
                // Comparar el imagePath sin importar mayúsculas y minúsculas
                return string.Equals(g.ImagePath, imagePath, StringComparison.OrdinalIgnoreCase);
            });

            return alreadyExists;
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error checking existence of image path: {ex.Message}");
            return false;
        }
    }

    public static int getGarmentID(string imagePath)
    {
        try
        {
            // Buscar la prenda con la misma ruta de imagen
            var existingGarment = dataManager.garmentsList.FirstOrDefault(g =>
                // Comparar el imagePath sin importar mayúsculas y minúsculas
                string.Equals(g.ImagePath, imagePath, StringComparison.OrdinalIgnoreCase));

            // Si la prenda existe, devolver su ID; de lo contrario, devolver -1
            return existingGarment != null ? existingGarment.GarmentID : -1;
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error checking existence of image path: {ex.Message}");
            return 0;
        }
    }

    private static bool isTypeValid(string type)
    {
        // Valid garment types
        string[] validTypes = { "Top", "Bottom", "Shoes" };
        return validTypes.Contains(type);
    }

    public static bool isGarmentEmpty(Garment garment)
    {
        // Si el nombre y la imagen están ambos vacíos, consideramos que el Garment está vacío.
        return string.IsNullOrEmpty(garment.Name) && string.IsNullOrEmpty(garment.ImagePath);
    }

    private static bool isNameUnique(string name)
    {
        try
        {
            var allGarments = dataManager.garmentsList;

            bool isNotUnique = allGarments.Any(g => string.Equals(g.Name, name, StringComparison.OrdinalIgnoreCase));

            return !isNotUnique;
        }
        catch (Exception ex)
        {
            Logger.Log(LogLevel.Error, $"Error checking uniqueness of garment name: {ex.Message}");
            return false;
        }
    }

    private static bool AreNameDifferent(List<Garment> garments)
    {
        HashSet<string> uniqueNames = new HashSet<string>();

        foreach (var garment in garments)
        {
            // Verificar si la ruta de imagen ya está en el conjunto
            if (!uniqueNames.Add(garment.Name))
            {
                Logger.Log(LogLevel.Error, $"Duplicate Name found: {garment.Name}");
                return false;
            }
        }

        return true;
    }

    private static bool AreImagePathsDifferent(List<Garment> garments)
    {
        HashSet<string> uniqueImagePaths = new HashSet<string>();

        foreach (var garment in garments)
        {
            // Verificar si la ruta de imagen ya está en el conjunto
            if (!uniqueImagePaths.Add(garment.ImagePath))
            {
                Logger.Log(LogLevel.Error, $"Duplicate ImagePath found: {garment.ImagePath}");
                return false;
            }
        }

        return true;
    }

    public static string IsOutfitDuplicate(List<Garment> garments)
    {
        foreach (var existingOutfit in dataManager.outfitsQueue)
        {
            // Verificar si existe la combinación de prendas (conjunto completo)
            if (OutfitExists(existingOutfit, garments))
            {
                // TODO: Show Popup saying "This outfit currently exists on database"
                Logger.Log(LogLevel.Error, Constants.DUPLICATED_OUTFIT_ERROR);
                return Constants.DUPLICATED_OUTFIT;
            }
        }
        return Constants.FALSE;
    }

    // Método que verifica si un conjunto de prendas ya existe en la base de datos
    private static bool OutfitExists(Outfit existingOutfit, List<Garment> garments)
    {
        // Comparar cada prenda en el conjunto existente con las nuevas prendas
        return GarmentExists(existingOutfit.TopID, garments[0], garments) &&
               GarmentExists(existingOutfit.BottomID, garments[1], garments) &&
               GarmentExists(existingOutfit.ShoesID, garments[2], garments);
    }

    private static bool GarmentExists(int existingGarmentID, Garment newGarment, List<Garment> garmentsList)
    {
        // Verificar si el garment ya existe y tiene el mismo ImagePath en la lista
        return existingGarmentID != -1 &&
               garmentsList.Any(existingGarment => existingGarment.GarmentID == existingGarmentID &&
                                                   existingGarment.ImagePath == newGarment.ImagePath);
    }

    // Method used before trying to delete an outfit (to know if is need to be deleted its garments from DB)
    private static bool IsGarmentAssociatedWithOutfit(int garmentID)
    {
        foreach (var outfit in dataManager.outfitsQueue)
        {
            if (outfit.TopID == garmentID || outfit.BottomID == garmentID || outfit.ShoesID == garmentID)
            {
                // El garment está asociado a este outfit
                return true;
            }
        }

        // El garment no está asociado a ningún outfit
        return false;
    }

    /*---------------------------------- ROLLBACK METHODS -------------------------------------*/
    public void BackupData()
    {
        // Llamar a BackupLocalData() y BackupDatabaseData() para respaldar ambos conjuntos de datos.
        BackupDataLocalData();
        BackupDataDBData();
    }

    private void BackupDataLocalData()
    {
        // Respaldar las estructuras de datos locales (garmentList y outfitsQueue).
    }
    private void BackupDataDBData()
    {
        // Respaldar los datos relevantes desde la base de datos SQL.
    }
    public void RollbackData()
    {
        // Implementar la lógica para revertir los datos en la base de datos SQL utilizando los respaldos de la base de datos.
        // Limpiar las estructuras de datos locales.
        // Restaurar los datos desde los respaldos locales.
        RollbackLocalData();
        RollbackDBData();
    }

    private void RollbackLocalData()
    {
        // Limpiar las estructuras de datos locales.
        // Restaurar los datos desde los respaldos locales.
     }
    private void RollbackDBData()
    {
        // Implementar la lógica para revertir los datos en la base de datos SQL utilizando los respaldos de la base de datos.
    }

}

/*--------------------------------- DATABASE TABLES --------------------------------- */
// Database table that contains every single Garment
[Table("Garment")]
public class Garment : IPrintable
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

    public string PrintInfo(string methodName)
    {
        return methodName + "()_" + $"Garment ID: {GarmentID}, Name: {Name}, Type: {Type}, ImagePath: {ImagePath}";
    }
}

// Database table that relates 3 garments in a unique outfit
[Table("Outfit")]
public class Outfit : IPrintable
{
    [PrimaryKey, AutoIncrement]
    public int OutfitID { get; set; }
    public int TopID { get; set; }
    public int BottomID { get; set; }
    public int ShoesID { get; set; }

    public string PrintInfo(string methodName)
    {
        return methodName + "()_" + $"Outfit ID: {OutfitID}, TopID: {TopID}, BottomID: {BottomID}, ShoesID: {ShoesID}";
    }
}

public interface IPrintable
{
    string PrintInfo(string methodName);
}
