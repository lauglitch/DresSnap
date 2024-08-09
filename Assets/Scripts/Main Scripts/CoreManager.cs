using UnityEngine;

public class CoreManager : MonoBehaviour
{
    private DatabaseManager databaseManagerInstance;
    private BackupDataManager backupDataManagerInstance;
    private GalleryLoader galleryLoaderInstance;
    private PopupManager popupManagerInstance;
    private DataManager dataManagerInstance;

    private static CoreManager instance;

    public static CoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Busca un objeto existente en la escena con CoreManager
                instance = FindObjectOfType<CoreManager>();

                if (instance == null)
                {
                    // Si no existe, crea uno nuevo
                    GameObject gameObject = new GameObject("CoreManager");
                    instance = gameObject.AddComponent<CoreManager>();
                }
                else
                {
                    // Si encontró uno, lo asigna
                    instance.InitializeManagers();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();

            Logger.Log(LogLevel.DeepTest, "CoreManager started.");
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        if (databaseManagerInstance == null)
            databaseManagerInstance = gameObject.AddComponent<DatabaseManager>();

        if (backupDataManagerInstance == null)
            backupDataManagerInstance = gameObject.AddComponent<BackupDataManager>();

        if (galleryLoaderInstance == null)
        {
            galleryLoaderInstance = FindObjectOfType<GalleryLoader>();
            if (galleryLoaderInstance == null)
            {
                 Logger.Log(LogLevel.Error, "GalleryLoader instance not found in the scene.");
            }
        }

        if (popupManagerInstance == null)
            popupManagerInstance = gameObject.AddComponent<PopupManager>();

        if (dataManagerInstance == null)
            dataManagerInstance = gameObject.AddComponent<DataManager>();
    }

    public DatabaseManager GetDatabaseManager() => databaseManagerInstance;
    public BackupDataManager GetBackupDataManager() => backupDataManagerInstance;
    public GalleryLoader GetGalleryLoader() => galleryLoaderInstance;
    public PopupManager GetPopupManager() => popupManagerInstance;
    public DataManager GetDataManager() => dataManagerInstance;
}
