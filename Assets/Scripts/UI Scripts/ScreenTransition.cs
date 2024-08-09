using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
public enum GarmentType
{
    Top,
    Bottom,
    Shoes
}
public class ScreenTransition : MonoBehaviour
{
    public GameObject mainMenuPanelCanvas;
    public GameObject createOutfitCanvas;
    public GameObject seeOutfitsCanvas;
    public GameObject outfitSavedPopup;
    public GameObject sureToExitFromCreateMenuPopup;
    public GameObject pickedOutfitPopup;
    public GameObject noOutfitsOnDBPopup;
    public GameObject sureToDeleteOutfitPopup;
    public GameObject editOutfitMenuPopup;

    // Only on Create Menu
    public GameObject top_Image;
    public GameObject bottom_Image;
    public GameObject shoes_Image;

    public GameObject topName_Input;
    public GameObject bottomName_Input;
    public GameObject shoesName_Input;
    private bool topNameInputInteractable = true;
    private bool bottomNameInputInteractable = true;
    private bool shoesNameInputInteractable = true;
    //

    // Only on See Menu
    public GameObject top_Image1;
    public GameObject bottom_Image1;
    public GameObject shoes_Image1;
    public GameObject top_Image2;
    public GameObject bottom_Image2;
    public GameObject shoes_Image2;
    public GameObject top_Image3;
    public GameObject bottom_Image3;
    public GameObject shoes_Image3;

    public Button nextButton;
    public Button prevButton;

    int[] outfitIdsArray;

    public Button editButton1;
    public Button deleteButton1;
    public Button editButton2;
    public Button deleteButton2;
    public Button editButton3;
    public Button deleteButton3;

    ///
    private List<List<Image>> pages = new List<List<Image>>();
    private int currentPageIndex;
    int totalPages;
    int outfitsPerPage;
    ///

    private string top_Path;
    private string bottom_Path;
    private string shoes_Path;

    private DatabaseManager databaseManager;
    private DataManager dataManager;
    private GalleryLoader galleryLoader;
    private PopupManager popupManager;

    private void Awake()
    {
      
    }

    void Start()
    {
        mainMenuPanelCanvas.SetActive(true);
        createOutfitCanvas.SetActive(false);
        seeOutfitsCanvas.SetActive(false);
        outfitSavedPopup.SetActive(false);
        sureToExitFromCreateMenuPopup.SetActive(false);
        pickedOutfitPopup.SetActive(false);
        noOutfitsOnDBPopup.SetActive(false);
        sureToDeleteOutfitPopup.SetActive(false);
        editOutfitMenuPopup.SetActive(false);

        currentPageIndex = 0;
        outfitsPerPage = 3;

        databaseManager = CoreManager.Instance.GetDatabaseManager();
        dataManager = CoreManager.Instance.GetDataManager();
        galleryLoader = CoreManager.Instance.GetGalleryLoader();
        popupManager = CoreManager.Instance.GetPopupManager();

        if (databaseManager == null)
        {
            Debug.LogError("Could not get DatabaseManager from CoreManager.Instance");
        }
        if (dataManager == null)
        {
            Debug.LogError("Could not get DataManager from CoreManager.Instance");
        }
        if (popupManager == null)
        {
            Debug.LogError("Could not get PopupManager from CoreManager.Instance");
        }

        Logger.Log(LogLevel.DeepTest, "ScreenTransition started.");
    }

    // ----------------------------------------------------------------------------------
    // ------------------------------- MAIN MENU BUTTONS                                      
    public void SeeOutfits_MainMenuButton()
    {
        // Check if there are outfits on database
        if (dataManager.outfitsQueue.Count == 0)
        {
            noOutfitsOnDBPopup.SetActive(true);
            Logger.Log(LogLevel.Warning, Constants.NO_OUTFITS_DB);
        }
        else
        {

            galleryLoader.SetDefaultImageAndText(targetImages: galleryLoader.targetSeeImages, targetTexts: galleryLoader.targetSeeTexts);

            seeOutfitsCanvas.SetActive(true);

            mainMenuPanelCanvas.SetActive(false);

            totalPages = (int)Math.Ceiling((double)dataManager.outfitsQueue.Count / outfitsPerPage);
            int lastPageOutfits = dataManager.outfitsQueue.Count % outfitsPerPage;

            Logger.Log(LogLevel.Test, Constants.TOTAL_PAGES_SEE_MENU + totalPages);

            LoadOutfitsForCurrentPage();
        }       
    }

    public void CreateOutfit_MainMenuButton()
    {
        //foreach (Image image in galleryLoader.targetCreateImages)
        //Debug.Log("ScreenTransition - targetCreateImages: " + image);

        //galleryLoader.targetCreateImages.ForEach(image => Debug.Log("CreateOutfit_MainMenuButton - targetCreateImages: " + image.gameObject.name));
        
        if (galleryLoader != null)
        {
            List<Image> createImages = galleryLoader.targetCreateImages;
            galleryLoader.SetDefaultImageAndText(targetImages: createImages, targetTexts: new List<GameObject>());
        }
        else
        {
            Debug.LogError("GalleryLoader reference is null!");
        }
        
        createOutfitCanvas.SetActive(true);

        mainMenuPanelCanvas.SetActive(false);
    }

    public void PickOutfit_MainMenuButtonAsync()
    {
        // Check if there are outfits on database
        if (dataManager.outfitsQueue.Count == 0)
        {
            noOutfitsOnDBPopup.SetActive(true);
            Logger.Log(LogLevel.Warning, Constants.NO_OUTFITS_DB);
        }
        else
        {
            galleryLoader.SetDefaultImageAndText(targetImages: galleryLoader.targetPickImages, targetTexts: galleryLoader.targetPickTexts);

            pickedOutfitPopup.SetActive(true);

            Outfit rndOutfit = DatabaseManager.PickRandomLocalOutfit();
            int rndTopID = rndOutfit.TopID, rndBottomID = rndOutfit.BottomID, rndShoesID = rndOutfit.ShoesID;
            Garment rndTop, rdnBottom, rdnShoes;
            string topName = Constants.UNDEFINED, topImagePath = Constants.UNDEFINED, bottomName = Constants.UNDEFINED,
                bottomImagePath = Constants.UNDEFINED, shoesName = Constants.UNDEFINED, shoesImagePath = Constants.UNDEFINED;

            if (rndTopID != -1)
            {
                rndTop = dataManager.garmentsList.FirstOrDefault(g => g.GarmentID == rndOutfit.TopID);
                if (rndTop != null)
                {
                    topName = rndTop.Name;
                    topImagePath = rndTop.ImagePath;
                }
                else
                {
                    // Este atuendo no tiene prenda superior
                }
            }

            if (rndBottomID != -1)
            {
                rdnBottom = dataManager.garmentsList.FirstOrDefault(g => g.GarmentID == rndOutfit.BottomID);
                if (rdnBottom != null)
                {
                    bottomName = rdnBottom.Name;
                    bottomImagePath = rdnBottom.ImagePath;
                }
                else
                {
                    // Este atuendo no tiene prenda inferior
                }
            }

            if (rndShoesID != -1)
            {
                rdnShoes = dataManager.garmentsList.FirstOrDefault(g => g.GarmentID == rndOutfit.ShoesID);
                if (rdnShoes != null)
                {
                    shoesName = rdnShoes.Name;
                    shoesImagePath = rdnShoes.ImagePath;
                }
                else
                {
                    // Este atuendo no tiene calzado
                }
            }

            // ...

            else
            {
                // This outfits has no shoes garment
            }

            Logger.Log(LogLevel.Test, Constants.RANDOM_PICKED_OUTFIT + Logger.OutfitString(rndOutfit));

            // DELETE: this is just for testing data
            /*String[] nameArray = { "Camiseta Kiss",  "Pantalones encerados", "Botas militares" }; 
            String[] pathArray = 
                {"C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/camisetakiss.PNG",
                "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/pantalonesencerados.PNG",
                "C:/Users/lrcor/Desktop/DEV/SOFTWARE/DresSnap_DEV/TestPhotos/botasmilitares.PNG"}; 
            */

            String[] nameArray = { topName, bottomName, shoesName };
            String[] pathArray = { topImagePath, bottomImagePath, shoesImagePath };

            // Show canvas with selected outfit
            galleryLoader.ProcessLocalData(nameArray, pathArray);
        }

    }

    public void ok_PickOutfitPopup()
    {
        pickedOutfitPopup.SetActive(false);

        galleryLoader.SetDefaultImageAndText(targetImages: galleryLoader.targetPickImages, targetTexts: galleryLoader.targetPickTexts);
    }

    public void ok_NoOutfitPopup()
    {
        noOutfitsOnDBPopup.SetActive(false);
    }

    public void ExitApp_MainMenuButton()
    {
        Application.Quit();
    }

    // ----------------------------------------------------------------------------------
    // ------------------------------- CREATE OUTFIT MENU BUTTONS 
    public void Create_CreateMenuButton(String type)
    {
        string imagePath = Constants.UNDEFINED;
        bool imagePathExists;

        switch (type)
        {
            case Constants.TOP_FIELD:
                imagePath = galleryLoader.LoadImageFromExplorer(Constants.TOP_FIELD);

                if (dataManager != null)
                {
                    // Accede a garmentsList aquí
                }
                else
                {
                    Logger.Log(LogLevel.Warning, "dataManager is null");
                }

                imagePathExists = dataManager.garmentsList.Any(garment => garment.ImagePath.Equals(imagePath));

                if (imagePathExists)
                {
                    foreach (Garment garment in dataManager.garmentsList)
                        if (garment.ImagePath.Equals(imagePath))
                        {
                            topNameInputInteractable = false;
                        }
                }
                else if (!imagePath.Equals(Constants.UNDEFINED))
                {
                    topNameInputInteractable = true;
                }

                break;
            case Constants.BOTTOM_FIELD:
                imagePath = galleryLoader.LoadImageFromExplorer(Constants.BOTTOM_FIELD);

                imagePathExists = dataManager.garmentsList.Any(garment => garment.ImagePath.Equals(imagePath));

                if (imagePathExists)
                {
                    foreach (Garment garment in dataManager.garmentsList)
                        if (garment.ImagePath.Equals(imagePath))
                            bottomName_Input.GetComponent<TMP_InputField>().interactable = false;
                }
                else
                    bottomName_Input.GetComponent<TMP_InputField>().interactable = true;

                break;
            case Constants.SHOES_FIELD:
                imagePath = galleryLoader.LoadImageFromExplorer(Constants.SHOES_FIELD);

                imagePathExists = dataManager.garmentsList.Any(garment => garment.ImagePath.Equals(imagePath));

                if (imagePathExists) 
                { 
                    foreach (Garment garment in dataManager.garmentsList)
                        if (garment.ImagePath.Equals(imagePath))
                            shoesName_Input.GetComponent<TMP_InputField>().interactable = false;
                }
                else
                    shoesName_Input.GetComponent<TMP_InputField>().interactable = true;

                break;
            default:
                break;
        }

        if (imagePath.Equals(Constants.UNDEFINED))
            Logger.Log(LogLevel.Warning, ("imagePath not obtained while Image Loading"));
        else
            Logger.Log(LogLevel.Test, ("Image loaded on " + type + ": " + imagePath));
    }

    public void CloseMenu_CreateMenuButton()
    {
        sureToExitFromCreateMenuPopup.GetComponent<PopupManager>().ShowPopup();
    }

    public async void SaveOutfit_CreateMenuButton()
    {
        bool canInsertOutfit = true;
        string validationReturnMessage = "";

        top_Path = galleryLoader.createTopImagePath;
        bottom_Path = galleryLoader.createBottomImagePath;
        shoes_Path = galleryLoader.createShoesImagePath;

        AndroidLogger.WriteLog("\nTopPath: " + top_Path + "\nBottom_Path: " + bottom_Path + "\nShoes_Path: " + shoes_Path);
        Logger.Log(LogLevel.DeepTest, "CreateMenu paths \nTopPath: " + top_Path + "\nBottom_Path: " + bottom_Path + "\nShoes_Path: " + shoes_Path);

        try
        {
            List<Garment> garmentList = new List<Garment>();
            string topName = topName_Input.GetComponent<TMP_InputField>().text;
            string bottomName = bottomName_Input.GetComponent<TMP_InputField>().text;
            string shoesName = shoesName_Input.GetComponent<TMP_InputField>().text;
            int[] garmentIDs = { -1, -1, -1 };

            Garment topGarment = new Garment
            {
                Name = topName,
                Type = "Top",
                ImagePath = string.IsNullOrEmpty(top_Path) ? Constants.NO_IMAGE_PATH : top_Path
            };
            Garment bottomGarment = new Garment
            {
                Name = bottomName,
                Type = "Bottom",
                ImagePath = string.IsNullOrEmpty(bottom_Path) ? Constants.NO_IMAGE_PATH : bottom_Path
            };
            Garment shoesGarment = new Garment
            {
                Name = shoesName,
                Type = "Shoes",
                ImagePath = string.IsNullOrEmpty(shoes_Path) ? Constants.NO_IMAGE_PATH : shoes_Path
            };

            garmentList.Add(topGarment);
            garmentList.Add(bottomGarment);
            garmentList.Add(shoesGarment);

            validationReturnMessage = await DatabaseManager.ValidateOutfitInfoAsync(garmentList);

            // If all garments are valid and outfit is not duplicated, proceed with inserting garments and outfit
            if (validationReturnMessage.Equals(Constants.VALID_OUTFIT))
            {
                bool canInsertGarment = false; // if garment currently exists, it won't be inserted

                // Associate original IDs to the 3 Garments to validate the complete Outfit
                garmentList[0].GarmentID = DatabaseManager.GetGarmentIDByImagePath(garmentList[0].ImagePath);
                garmentList[1].GarmentID = DatabaseManager.GetGarmentIDByImagePath(garmentList[1].ImagePath);
                garmentList[2].GarmentID = DatabaseManager.GetGarmentIDByImagePath(garmentList[2].ImagePath);

                // Create a backup of the local database
                databaseManager.BackupData();

                // If the outfit is not a duplicate, proceed with inserting garments and outfit
                if (DatabaseManager.IsOutfitDuplicate(garmentList).Equals(Constants.FALSE))
                {
                    try
                    {
                        // Insert Garment/s
                        foreach (Garment garment in garmentList)
                        {
                            int idIndex = -1;

                            if (garment.Type.Equals(Constants.TOP_FIELD))
                                idIndex = 0;
                            else if (garment.Type.Equals(Constants.BOTTOM_FIELD))
                                idIndex = 1;
                            else if (garment.Type.Equals(Constants.SHOES_FIELD))
                                idIndex = 2;
                            else
                                Logger.Log(LogLevel.Error, Constants.NO_GARMENT_TYPE_ERROR);

                            if (DatabaseManager.areGarmentsGoingToBeSaved[idIndex])
                                canInsertGarment = true;
                            else
                                canInsertGarment = false;

                            try
                            {
                                if (canInsertGarment)
                                {
                                    int garmentID = await DatabaseManager.InsertGarmentAsync(garment);

                                    if (garmentID != -1)
                                        garmentIDs[idIndex] = garmentID;
                                    else
                                    {
                                        Logger.Log(LogLevel.Error, Constants.GARMENT_VALIDATION_FAILED_ERROR);
                                        canInsertOutfit = false;
                                        break;  // Exit the loop if any garment insertion fails
                                    }
                                }
                                else
                                {
                                    garmentIDs[idIndex] = DatabaseManager.GetGarmentIDByImagePath(garment.ImagePath);
                                }
                            }
                            catch (Exception e)
                            {
                                Logger.Log(LogLevel.Error, Constants.GARMENT_INSERTION_FAILED_ERROR + e);
                            }
                        }

                        // If all garments are inserted successfully, insert the outfit
                        if (canInsertOutfit)
                        {
                            Outfit newOutfit = new Outfit
                            {
                                TopID = garmentList[0].GarmentID,
                                BottomID = garmentList[1].GarmentID,
                                ShoesID = garmentList[2].GarmentID
                            };

                            int outfitID = await DatabaseManager.InsertOutfitAsync(newOutfit);

                            newOutfit.OutfitID = outfitID;

                            canInsertOutfit = (outfitID != -1);

                            // Show success message
                            outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = Constants.OUTFIT_SAVED;
                        }
                        else
                        {
                            // Log an error if outfit validation fails
                            Logger.Log(LogLevel.Error, Constants.OUTFIT_VALIDATION_FAILED_ERROR);
                        }
                    }
                    catch (Exception e)
                    {
                        // Log an error if there is an exception during the process
                        Logger.Log(LogLevel.Error, Constants.GARMENT_INSERTION_FAILED_ERROR + e.Message);
                    }
                }
                else
                {
                    // Rollback the local database to the backup state
                    databaseManager.RollbackData();

                    // Show an error message for a duplicated outfit
                    outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = Constants.DUPLICATED_OUTFIT_ERROR;
                    canInsertOutfit = false;
                }
            }
            else if (validationReturnMessage.Equals(Constants.MISSING_GARMENTS))
            {
                // Show an error message for missing garments
                outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = Constants.MISSING_GARMENTS_ERROR;
                canInsertOutfit = false;
            }
            else if (validationReturnMessage.Equals(Constants.SAME_IMAGE))
            {
                // Show an error message for garments with the same image
                outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = Constants.SAME_GARMENT_IMAGE_ERROR;
                canInsertOutfit = false;
            }
            else if (validationReturnMessage.Equals(Constants.EXISTING_GARMENT_NAME_ERROR))
            {
                // Show an error message for garments with existing name
                outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = Constants.EXISTING_GARMENT_NAME_ERROR;
                canInsertOutfit = false;
            }
            else if (validationReturnMessage.Equals(Constants.EMPTY_GARMENT_NAME_ERROR))
            {
                // Show an error message for garments with repeated image
                outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = Constants.EMPTY_GARMENT_NAME_ERROR;
                canInsertOutfit = false;
            }
            // This error should not be shown to user
            else if (validationReturnMessage.Equals(Constants.NO_GARMENT_TYPE_ERROR))
            {
                // Show an error message for garments with the same image
                outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = Constants.NO_GARMENT_TYPE_ERROR;
                canInsertOutfit = false;
            }
        }
        catch (Exception e)
        {
            // Log an error if there is an exception during the process
            Logger.Log(LogLevel.Error, Constants.INVALID_USER_OUTFIT_ERROR + e.Message);
            canInsertOutfit = false;
        }

        // IMPORTANT TO UPDATE LOCAL DATA STRUCTURES TO WORK WITH CURRENT DATA
        await DatabaseManager.UpdateLocalDataStructures();

        // Show the outfit saved popup
        outfitSavedPopup.GetComponent<PopupManager>().ShowPopup();
    }

    public void ok_CreateMenuButton()
    {
        // TODO: Reset Images and Inputs on Create Menu

        outfitSavedPopup.GetComponent<PopupManager>().HidePopup();
    }

    public void yes_CreateMenuButton()
    {
        sureToExitFromCreateMenuPopup.GetComponent<PopupManager>().HidePopup();
        mainMenuPanelCanvas.SetActive(true);
        createOutfitCanvas.SetActive(false);

        deleteWrittenInfoOnCreateMenu();
    }

    public void no_CreateMenuButton()
    {
        sureToExitFromCreateMenuPopup.GetComponent<PopupManager>().HidePopup();
    }

    // ----------------------------------------------------------------------------------
    // ------------------------------- SEE MENU BUTTONS                 
    public void EditOutfit_SeeMenuButton(int row)
    {
        editOutfitMenuPopup.SetActive(true);
        string popupName = "editOutfitMenuPopup";

        switch (row)
        {
            case 1:
                //Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[0] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[0], popupName);
                break;
            case 2:
                //Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[1] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[1], popupName);
                break;
            case 3:
                //Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[2] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[2], popupName);
                break;
            default:
                Logger.Log(LogLevel.Error, Constants.OUTFIT_SELECTION_ON_EDIT_ERROR);
                break;
        }
    }

    public void ConfirmEdition_SeeMenuButton()
    {
        // IMPORTANT TO UPDATE LOCAL DATA STRUCTURES TO WORK WITH CURRENT DATA
        DatabaseManager.UpdateLocalDataStructures();
    }

    public void CancelEdition_SeeMenuButton()
    {
        editOutfitMenuPopup.SetActive(false);
        SetDefaultImages(galleryLoader.targetEditImages, 0, 2);
    }

    public void DeleteOutfit_SeeMenuButton(int row)
    {
        sureToDeleteOutfitPopup.SetActive(true);
        string popupName = Constants.SURE_TO_DELETE_OUTFIT_POPUP;

        switch (row)
        {
            case 1:
                Logger.Log(LogLevel.DeepTest, Constants.SELECTED_OUTFIT_TO_EDIT + outfitIdsArray[0]);
                LoadOutfitForPopups(outfitIdsArray[0], popupName);
                break;
            case 2:
                Logger.Log(LogLevel.DeepTest, Constants.SELECTED_OUTFIT_TO_EDIT + outfitIdsArray[1]);
                LoadOutfitForPopups(outfitIdsArray[1], popupName);
                break;
            case 3:
                Logger.Log(LogLevel.DeepTest, Constants.SELECTED_OUTFIT_TO_EDIT + outfitIdsArray[2]);
                LoadOutfitForPopups(outfitIdsArray[2], popupName);
                break;
            default:
                Logger.Log(LogLevel.Error, Constants.OUTFIT_SELECTION_ON_DELETE_ERROR);
                break;
        }
    }

    public void ConfirmDeletion_SeeMenuButton()
    {
        // IMPORTANT TO UPDATE LOCAL DATA STRUCTURES TO WORK WITH CURRENT DATA
        DatabaseManager.UpdateLocalDataStructures();
    }

    public void CancelDeletion_SeeMenuButton()
    {
        sureToDeleteOutfitPopup.SetActive(false);
        SetDefaultImages(galleryLoader.targetDeleteImages, 0, 2);
    }

    public void CloseMenu_SeeMenuButton()
    {
        mainMenuPanelCanvas.SetActive(true);
        seeOutfitsCanvas.SetActive(false);
        currentPageIndex = 0;
    }

    public void ChangePage(string direction)
    {
        int lastPage = currentPageIndex;
        int pageDirection = 0;

        // Convert string direction to integer direction value
        if (direction.Equals("Left", StringComparison.OrdinalIgnoreCase))
        {
            pageDirection = -1;
        }
        else if (direction.Equals("Right", StringComparison.OrdinalIgnoreCase))
        {
            pageDirection = 1;
        }
        else
        {
            Logger.Log(LogLevel.Warning, Constants.INVALID_DIRECTION_ERROR);
            return;
        }

        // Update the page index with the given address
        currentPageIndex += pageDirection;

        // Make sure the page index is in a valid range
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, totalPages - 1);

        Logger.Log(LogLevel.DeepTest, Constants.CURRENT_PAGE_SEE_MENU + currentPageIndex);

        // Update images with those of the new page
        if (lastPage != currentPageIndex)
            LoadOutfitsForCurrentPage();
    }

    // ------------------------------- SEE MENU LOGIC          
    private void LoadOutfitsForCurrentPage()
    {
        // Convert the outfits queue to a list
        List<Outfit> outfitsList = dataManager.outfitsQueue.ToList();

        // Get the outfits for the current page
        int startIndex = currentPageIndex * outfitsPerPage;
        int endIndex = Mathf.Min(startIndex + outfitsPerPage, outfitsList.Count);

        List<Outfit> outfitsForPage = outfitsList.GetRange(startIndex, endIndex - startIndex);

        outfitIdsArray = outfitsForPage.Select(outfit => outfit.OutfitID).ToArray();

        // Calculate the number of empty slots on the current page
        int remainingSlots = Mathf.Max(0, 3 - outfitsForPage.Count);

        // Call SetDefaultImage for the remaining slots if there are no outfits on the current page
        if (remainingSlots > 0)
        {
            Logger.Log(LogLevel.DeepTest, Constants.REMAINING_SLOTS_SEE_MENU + remainingSlots);

            int startIdx = 6 - remainingSlots * 3;
            SetDefaultImages(galleryLoader.targetSeeImages, startIdx, 9);

            if (remainingSlots == 1)
            {
                EnableEditDeleteButtons(editButton1, deleteButton1);
                EnableEditDeleteButtons(editButton2, deleteButton2);

                DisableEditDeleteButtons(editButton3, deleteButton3);
            } else if (remainingSlots == 2)
            {
                EnableEditDeleteButtons(editButton1, deleteButton1);

                DisableEditDeleteButtons(editButton2, deleteButton2);
                DisableEditDeleteButtons(editButton3, deleteButton3);
            }
        }
        else
        {
            Logger.Log(LogLevel.DeepTest, Constants.REMAINING_SLOTS_SEE_MENU + 0);

            EnableEditDeleteButtons(editButton1, deleteButton1);
            EnableEditDeleteButtons(editButton2, deleteButton2);
            EnableEditDeleteButtons(editButton3, deleteButton3);
        }


        // Build a list of image paths for all garments on the current page
        List<string> imagePaths = new List<string>();
        foreach (Outfit outfit in outfitsForPage)
        {
            // Get the image paths from garmentsDictionary using the IDs of garments in the outfit
            string topImagePath = GetImagePathForGarment(outfit.TopID);
            string bottomImagePath = GetImagePathForGarment(outfit.BottomID);
            string shoesImagePath = GetImagePathForGarment(outfit.ShoesID);

            imagePaths.Add(topImagePath);
            imagePaths.Add(bottomImagePath);
            imagePaths.Add(shoesImagePath);
        }

        StartCoroutine(LoadImagesForGarments(galleryLoader.targetSeeImages, imagePaths));
    }

    private void LoadOutfitForPopups(int id, string popupName)
    {
        // Search outfit with the parameter id
        Outfit outfit = dataManager.outfitsQueue.FirstOrDefault(outfit => outfit.OutfitID == id);

        if (outfit == null)
            Logger.Log(LogLevel.Error, (Constants.OUTFIT_NOT_FOUND_TO_EDIT_ERROR + id));
        else
            Logger.Log(LogLevel.DeepTest, (Constants.OUTFIT_FOUND_TO_EDIT + id));


        // Build a list of image paths for all garments of the selected outfit
        List<string> imagePaths = new List<string>();

        // Get the image paths from garmentsDictionary using the ID of garment in the outfit
        string topImagePath = GetImagePathForGarment(outfit.TopID);
        string bottomImagePath = GetImagePathForGarment(outfit.BottomID);
        string shoesImagePath = GetImagePathForGarment(outfit.ShoesID);

        imagePaths.Add(topImagePath);
        imagePaths.Add(bottomImagePath);
        imagePaths.Add(shoesImagePath);

        if (popupName.Equals(Constants.SURE_TO_DELETE_OUTFIT_POPUP))
            StartCoroutine(LoadImagesForGarments(galleryLoader.targetDeleteImages, imagePaths));
        else if (popupName.Equals(Constants.EDIT_OUTFIT_MENU_POPUP))
            StartCoroutine(LoadImagesForGarments(galleryLoader.targetEditImages, imagePaths));
        else
            Logger.Log(LogLevel.Error, Constants.INVALID_POPUP_NAME_ERROR);

    }

    private string GetImagePathForGarment(int garmentID)
    {
        if (garmentID.Equals(Constants.NO_GARMENT_ID))
        {
            return Constants.NO_IMAGE_PATH;
        }
        else
        {
            // Get the garment from garmentsList using LINQ
            Garment garment = dataManager.garmentsList.FirstOrDefault(g => g.GarmentID == garmentID);

            if (garment != null)
            {
                return garment.ImagePath;
            }
            else
            {
                Logger.Log(LogLevel.Error, Constants.NOT_FOUND_GARMENT_ERROR);
                return "";
            }
        }
    }

    private IEnumerator LoadImagesForGarments(List<Image> targetImages, List<string> imagePaths)
    {
        Logger.Log(LogLevel.DeepTest, "targetImages size: " + targetImages.Count + " // imagePaths size: " + imagePaths.Count);

        // Iterate through image paths
        for (int i = 0; i < imagePaths.Count; i++)
        {
            // Get the corresponding image by index
            Image targetImage = targetImages[i];
            if (targetImage != null)
            {
                // Load the image into the Image object
                yield return StartCoroutine(galleryLoader.LoadImageFromPath(targetImage, imagePaths[i]));
            }
            else
            {
                Logger.Log(LogLevel.Error, Constants.INVALID_GARMENT_INDEX_ERROR + i);
            }
        }
    }

    private void SetDefaultImages(List<Image> targetImages, int startIdx, int endIdx)
    {
        for (int i = startIdx; i < endIdx; i++)
        {
            galleryLoader.SetDefaultImage(targetImages[i]);
            Logger.Log(LogLevel.DeepTest, $"TargetSeeImages.name: {galleryLoader.targetSeeImages[i].name}");
        }
    }

    private void DisableEditDeleteButtons(Button editButton, Button deleteButton)
    {
        // Deshabilita los botones "Edit" y "Delete"
        editButton.interactable = false;
        deleteButton.interactable = false;
    }

    private void EnableEditDeleteButtons(Button editButton, Button deleteButton)
    {
        // Habilita los botones "Edit" y "Delete"
        editButton.interactable = true;
        deleteButton.interactable = true;
    }
    
    // ----------------------------------------------------------------------------------
    // ------------------------------- IN APP UTILITIES                                       
    public void deleteWrittenInfoOnCreateMenu()
    {
        // Delete Image content on Create Menu
        top_Image.GetComponent<Image>().sprite = null;
        bottom_Image.GetComponent<Image>().sprite = null;
        shoes_Image.GetComponent<Image>().sprite = null;

        // Delete Input content on Create Menu
        topName_Input.GetComponent<TMP_InputField>().text = string.Empty;
        bottomName_Input.GetComponent<TMP_InputField>().text = string.Empty;
        shoesName_Input.GetComponent<TMP_InputField>().text = string.Empty;
    }
}

