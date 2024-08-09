using UnityEngine;
public static class ManagerFactory
{
    public static ScreenTransition CreateScreenTransition()
    {
        GameObject screenTransitionObject = new GameObject("ScreenTransition");
        return screenTransitionObject.AddComponent<ScreenTransition>();
    }

    public static DatabaseManager CreateDatabaseManager()
    {
        GameObject databaseManagerObject = new GameObject("DatabaseManager");
        return databaseManagerObject.AddComponent<DatabaseManager>();
    }

    public static BackupDataManager CreateBackupDataManager()
    {
        GameObject backupDataManagerObject = new GameObject("BackupDataManager");
        return backupDataManagerObject.AddComponent<BackupDataManager>();
    }

    public static GalleryLoader CreateGalleryLoader()
    {
        GameObject galleryLoaderObject = new GameObject("GalleryLoader");
        return galleryLoaderObject.AddComponent<GalleryLoader>();
    }

    public static PopupManager CreatePopupManager()
    {
        GameObject popupManagerObject = new GameObject("PopupManager");
        return popupManagerObject.AddComponent<PopupManager>();
    }

    public static DataManager CreateDataManager()
    {
        GameObject dataManagerObject = new GameObject("DataManager");
        return dataManagerObject.AddComponent<DataManager>();
    }
}

