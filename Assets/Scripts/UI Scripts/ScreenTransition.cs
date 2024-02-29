using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

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

    public GalleryLoader galleryLoader;

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
    }

    // ----------------------------------------------------------------------------------
    // ------------------------------- MAIN MENU BUTTONS                                      
    public void SeeOutfits_MainMenuButton()
    {
        // Check if there are outfits on database
        if (DatabaseManager.outfitsQueue.Count == 0)
        {
            noOutfitsOnDBPopup.SetActive(true);
            Logger.Log(LogLevel.Warning, "There are no outfits on database");
        }
        else
        {

            galleryLoader.SetDefaultImageAndText(targetImages: galleryLoader.targetSeeImages, targetTexts: galleryLoader.targetSeeTexts);

            seeOutfitsCanvas.SetActive(true);

            mainMenuPanelCanvas.SetActive(false);

            totalPages = (int)Math.Ceiling((double)DatabaseManager.outfitsQueue.Count / outfitsPerPage);
            int lastPageOutfits = DatabaseManager.outfitsQueue.Count % outfitsPerPage;

            Logger.Log(LogLevel.DeepTest, "Totalpages on See Menu: " + totalPages);

            LoadOutfitsForCurrentPage();
        }       
    }

    public void CreateOutfit_MainMenuButton()
    {
        galleryLoader.SetDefaultImageAndText(targetImages: galleryLoader.targetCreateImages, targetTexts: new List<GameObject>());

        createOutfitCanvas.SetActive(true);

        mainMenuPanelCanvas.SetActive(false);
    }

    // TODO: should be async?
    public async void PickOutfit_MainMenuButtonAsync()
    {
        // Check if there are outfits on database
        if (DatabaseManager.outfitsQueue.Count == 0)
        {
            noOutfitsOnDBPopup.SetActive(true);
            Logger.Log(LogLevel.Warning, "There are no outfits on database");
        }
        else
        {
            galleryLoader.SetDefaultImageAndText(targetImages: galleryLoader.targetPickImages, targetTexts: galleryLoader.targetPickTexts);

            pickedOutfitPopup.SetActive(true);

            Outfit rndOutfit = DatabaseManager.PickRandomLocalOutfit();
            int rndTopID = rndOutfit.TopID, rndBottomID = rndOutfit.BottomID, rndShoesID = rndOutfit.ShoesID;
            Garment rndTop, rdnBottom, rdnShoes;
            string topName = DatabaseManager.UNDEFINED, topImagePath = DatabaseManager.UNDEFINED, bottomName = DatabaseManager.UNDEFINED,
                bottomImagePath = DatabaseManager.UNDEFINED, shoesName = DatabaseManager.UNDEFINED, shoesImagePath = DatabaseManager.UNDEFINED;


            if (rndTopID != -1)
            {
                DatabaseManager.garmentsDictionary.TryGetValue(rndOutfit.TopID, out rndTop);

                topName = rndTop.Name;
                topImagePath = rndTop.ImagePath;
            }
            else
            {
                // This outfits has no top garment
            }

            if (rndBottomID != -1)
            {
                DatabaseManager.garmentsDictionary.TryGetValue(rndOutfit.BottomID, out rdnBottom);

                bottomName = rdnBottom.Name;
                bottomImagePath = rdnBottom.ImagePath;
            }
            else
            {
                // This outfits has no bottom garment
            }

            if (rndShoesID != -1)
            {
                DatabaseManager.garmentsDictionary.TryGetValue(rndOutfit.ShoesID, out rdnShoes);

                shoesName = rdnShoes.Name;
                shoesImagePath = rdnShoes.ImagePath;
            }
            else
            {
                // This outfits has no shoes garment
            }

            Logger.Log(LogLevel.DeepTest, "Random picked outfit: " + Logger.OutfitString(rndOutfit));

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
        switch (type)
        {
            case "Top":
                galleryLoader.LoadImageFromExplorer("Top");
                break;
            case "Bottom":
                galleryLoader.LoadImageFromExplorer("Bottom");
                break;
            case "Shoes":
                galleryLoader.LoadImageFromExplorer("Shoes");
                break;
            default:
                break;
        }
    }

    public void CloseMenu_CreateMenuButton()
    {
        sureToExitFromCreateMenuPopup.GetComponent<PopupManager>().ShowPopup();
    }

    public async void SaveOutfit_CreateMenuButton()
    {
        bool everythingOk = true;

        top_Path = galleryLoader.createTopImagePath;
        bottom_Path = galleryLoader.createBottomImagePath;
        shoes_Path = galleryLoader.createShoesImagePath;

        AndroidLogger.WriteLog("\nTopPath: " + top_Path + "\nBottom_Path: " + bottom_Path + "\nShoes_Path: " + shoes_Path);

        try
        {
            List<Garment> garmentList = new List<Garment>();
            string topName = topName_Input.GetComponent<TMP_InputField>().text;
            string bottomName = bottomName_Input.GetComponent<TMP_InputField>().text;
            string shoesName = shoesName_Input.GetComponent<TMP_InputField>().text;

            Garment topGarment = new Garment
            {
                Name = topName,
                Type = "Top",
                ImagePath = string.IsNullOrEmpty(top_Path) ? DatabaseManager.NO_IMAGE_PATH : top_Path
            };

            Garment bottomGarment = new Garment
            {
                Name = bottomName,
                Type = "Bottom",
                ImagePath = string.IsNullOrEmpty(bottom_Path) ? DatabaseManager.NO_IMAGE_PATH : bottom_Path
            };

            Garment shoesGarment = new Garment
            {
                Name = shoesName,
                Type = "Shoes",
                ImagePath = string.IsNullOrEmpty(shoes_Path) ? DatabaseManager.NO_IMAGE_PATH : shoes_Path
            };
            //Garment topGarment = new Garment { Name = topName_Input.GetComponent<TMP_InputField>().text, Type = "Top", ImagePath = top_Path };
            //Garment bottomGarment = new Garment { Name = bottomName_Input.GetComponent<TMP_InputField>().text, Type = "Bottom", ImagePath = bottom_Path };
            //Garment shoesGarment = new Garment { Name = shoesName_Input.GetComponent<TMP_InputField>().text, Type = "Shoes", ImagePath = shoes_Path };

            // If garments are not null, add them to a List<Garment>
            if (!DatabaseManager.GarmentIsEmpty(topGarment))
                garmentList.Add(topGarment);
            else
                Logger.Log(LogLevel.Warning, "Top garment is empty. Not saving or associating with an outfit.");
            if (!DatabaseManager.GarmentIsEmpty(bottomGarment))
                garmentList.Add(bottomGarment);
            else
                Logger.Log(LogLevel.Warning, "Bottom garment is empty. Not saving or associating with an outfit.");
            if (!DatabaseManager.GarmentIsEmpty(shoesGarment))
                garmentList.Add(shoesGarment);
            else
                Logger.Log(LogLevel.Warning, "Shoes garment is empty. Not saving or associating with an outfit.");

            // Check if complete outfit is valid
            bool outfitInfoValid = await DatabaseManager.ValidateOutfitInfoAsync(garmentList);

            // If all validations pass, proceed with inserting garments and outfit
            if (outfitInfoValid)
            {
                List<int> garmentIDs = new List<int>();

                // Insert Garment/s
                foreach (var garment in garmentList)
                {
                    int garmentID = await DatabaseManager.InsertGarmentAsync(garment);

                    if (garmentID != -1)
                    {
                        garmentIDs.Add(garmentID);
                    }
                    else
                    {
                        Logger.Log(LogLevel.Error, "Garment info validation failed.");
                        everythingOk = false;
                        break;  // Exit the loop if any garment insertion fails
                    }
                }

                // Insert Outfit
                if (everythingOk)
                {
                    Outfit newOutfit = new Outfit
                    {
                        TopID = garmentIDs.Count > 0 ? garmentIDs[0] : DatabaseManager.NO_GARMENT_ID,
                        BottomID = garmentIDs.Count > 1 ? garmentIDs[1] : DatabaseManager.NO_GARMENT_ID,
                        ShoesID = garmentIDs.Count > 2 ? garmentIDs[2] : DatabaseManager.NO_GARMENT_ID
                    };

                    int outfitID = await DatabaseManager.InsertOutfitAsync(newOutfit);
                    newOutfit.OutfitID = outfitID;

                    everythingOk = (outfitID != -1);

                    // IMPORTANT TO UPDATE LOCAL DATA STRUCTURES TO WORK WITH CURRENT DATA
                    DatabaseManager.UpdateLocalDataStructures();
                }
                else
                {
                    everythingOk = false;
                    Logger.Log(LogLevel.Error, "Outfit info validation failed.");
                }
            } else
            {
                everythingOk = false;
            }

        }
        catch (Exception e)
        {
            Logger.Log(LogLevel.Error, $"Error recollecting info about new outfit: {e.Message}");
            everythingOk = false;
        }

        if (everythingOk)
        {
            outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = "Outfit saved successfully";
        }
        else
        {
            outfitSavedPopup.GetComponentInChildren<TMP_Text>().text = "Error saving Outfit. Try again";
        }

        outfitSavedPopup.GetComponent<PopupManager>().ShowPopup();

        //await DatabaseManager.PrintAllOutfitsOnLocal();
        //await DatabaseManager.PrintAllGarmentsOnLocal();
    }

    public void ok_CreateMenuButton()
    {
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
                Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[0] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[0], popupName);
                break;
            case 2:
                Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[1] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[1], popupName);
                break;
            case 3:
                Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[2] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[2], popupName);
                break;
            default:
                Logger.Log(LogLevel.Error, "Error on selecting Outfit row when 'Edit' button is pressed");
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
        string popupName = "sureToDeleteOutfitPopup";

        switch (row)
        {
            case 1:
                Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[0] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[0], popupName);
                break;
            case 2:
                Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[1] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[1], popupName);
                break;
            case 3:
                Logger.Log(LogLevel.DeepTest, "Outfit " + outfitIdsArray[2] + " selected to edit");
                LoadOutfitForPopups(outfitIdsArray[2], popupName);
                break;
            default:
                Logger.Log(LogLevel.Error, "Error on selecting Outfit row when 'Delete' button is pressed");
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
            Logger.Log(LogLevel.Warning, "Invalid direction parameter. Use 'Left' or 'Right'.");
            return;
        }

        // Update the page index with the given address
        currentPageIndex += pageDirection;

        // Make sure the page index is in a valid range
        currentPageIndex = Mathf.Clamp(currentPageIndex, 0, totalPages - 1);

        Logger.Log(LogLevel.DeepTest, "Current page: " + currentPageIndex);

        // Update images with those of the new page
        if (lastPage != currentPageIndex)
            LoadOutfitsForCurrentPage();
    }

    // ------------------------------- SEE MENU LOGIC          
    private void LoadOutfitsForCurrentPage()
    {
        // Convert the outfits queue to a list
        List<Outfit> outfitsList = DatabaseManager.outfitsQueue.ToList();

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
            Logger.Log(LogLevel.DeepTest, "Remaining Slots: " + remainingSlots);

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
            Logger.Log(LogLevel.DeepTest, "Remaining Slots: 0");

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
        Outfit outfit = DatabaseManager.outfitsQueue.FirstOrDefault(outfit => outfit.OutfitID == id);

        if (outfit == null)
            Logger.Log(LogLevel.Error, ("Outfit to edit with id=" + id + " was not found on database"));
        else
            Logger.Log(LogLevel.DeepTest, ("Outfit to edit with id=" + id + " was found on database"));


        // Build a list of image paths for all garments of the selected outfit
        List<string> imagePaths = new List<string>();

        // Get the image paths from garmentsDictionary using the ID of garment in the outfit
        string topImagePath = GetImagePathForGarment(outfit.TopID);
        string bottomImagePath = GetImagePathForGarment(outfit.BottomID);
        string shoesImagePath = GetImagePathForGarment(outfit.ShoesID);

        imagePaths.Add(topImagePath);
        imagePaths.Add(bottomImagePath);
        imagePaths.Add(shoesImagePath);

        if (popupName.Equals("sureToDeleteOutfitPopup"))
            StartCoroutine(LoadImagesForGarments(galleryLoader.targetDeleteImages, imagePaths));
        else if (popupName.Equals("editOutfitMenuPopup"))
            StartCoroutine(LoadImagesForGarments(galleryLoader.targetEditImages, imagePaths));
        else
            Logger.Log(LogLevel.Error, "Popup name not valid");

    }

    private string GetImagePathForGarment(int garmentID)
    {
        if (garmentID.Equals(DatabaseManager.NO_GARMENT_ID)){
            return DatabaseManager.NO_IMAGE_PATH;
        } else
        {
            // Get the garment from garmentsDictionary using the ID
            if (DatabaseManager.garmentsDictionary.TryGetValue(garmentID, out Garment garment))
                return garment.ImagePath;
            else
            {
                Logger.Log(LogLevel.Error, "Garment with ID " + garmentID + " not found in garmentsDictionary.");
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
                Logger.Log(LogLevel.Error, "Invalid garment index: " + i);
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

