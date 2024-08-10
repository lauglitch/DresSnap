using UnityEngine;

public class CoreManager : MonoBehaviour
{
    public DatabaseManager databaseManagerInstance;
    public BackupDataManager backupDataManagerInstance;
    public GalleryLoader galleryLoaderInstance;
    public PopupManager popupManagerInstance;
    public DataManager dataManagerInstance;
    public PermissionManager permisionManagerInstance;

    private static CoreManager instance;

    public static CoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CoreManager>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject("CoreManager");
                    instance = gameObject.AddComponent<CoreManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        Logger.Log(LogLevel.DeepTest, "CoreManager Awake() method called");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeManagers()
    {
        /*
        databaseManagerInstance = gameObject.AddComponent<DatabaseManager>();
        backupDataManagerInstance = gameObject.AddComponent<BackupDataManager>();

        galleryLoaderInstance = FindObjectOfType<GalleryLoader>();
        if (galleryLoaderInstance == null)
        {
            Logger.Log(LogLevel.Error, "GalleryLoader instance not found in the scene.");
        }

        popupManagerInstance = gameObject.AddComponent<PopupManager>();
        dataManagerInstance = gameObject.AddComponent<DataManager>();

        Logger.Log(LogLevel.DeepTest, "Managers initialized.");
        */
    }

    public DatabaseManager GetDatabaseManager() => databaseManagerInstance;
    public BackupDataManager GetBackupDataManager() => backupDataManagerInstance;
    public GalleryLoader GetGalleryLoader() => galleryLoaderInstance;
    public PopupManager GetPopupManager() => popupManagerInstance;
    public DataManager GetDataManager() => dataManagerInstance;
    public PermissionManager GetPermissionManager() => permisionManagerInstance;
}
