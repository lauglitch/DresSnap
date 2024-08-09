
public class Constants
{
    // GAMEOBJECT NAMES
    public const string SURE_TO_DELETE_OUTFIT_POPUP = "sureToDeleteOutfitPopup";
    public const string EDIT_OUTFIT_MENU_POPUP = "editOutfitMenuPopup";

    // TABLE NAMES
    public const string GARMENT_TABLE_NAME = "Garment";
    public const string OUTFIT_TABLE_NAME = "Outfit";

    // FIELD NAMES
    public const string TOP_FIELD = "Top";
    public const string BOTTOM_FIELD = "Bottom";
    public const string SHOES_FIELD = "Shoes";
    // TODO: FIELD CONTENT COMPARISONS
    public const int NO_GARMENT_ID = -1;
    public const string NO_IMAGE_PATH = "undefined";
    public const string UNDEFINED = "undefined";

    // SUCCESS
    public const string VALID_GARMENT = "validGarment";
    public const string VALID_OUTFIT = "validOutfit";

    // UI ERRORS
    public const string SAME_GARMENT_IMAGE_ERROR = "You can not repeat images";
    public const string SAME_GARMENT_NAME_ERROR = "You can not repeat names";
    public const string EXISTING_GARMENT_NAME_ERROR = "Garment names must be new";
    public const string EMPTY_GARMENT_NAME_ERROR = "Garment names can not be empty";
    public const string MISSING_GARMENTS_ERROR = "You need 3 garments to save the outfit";
    public const string DUPLICATED_OUTFIT_ERROR = "This outfit already exists";
    public const string LOAD_IMAGE_ERROR = "Error loading texture: ";
    public const string NOT_FOUND_TEXT_COMPONENT_ERROR = "Text component not found in object: ";
    public const string INVALID_USER_OUTFIT_ERROR = "Error recollecting info about new outfit: ";
    public const string OUTFIT_SELECTION_ON_EDIT_ERROR = "Error on selecting Outfit row when 'Edit' button is pressed";
    public const string OUTFIT_SELECTION_ON_DELETE_ERROR = "Error on selecting Outfit row when 'Delete' button is pressed";
    public const string INVALID_DIRECTION_ERROR = "Invalid direction parameter. Use 'Left' or 'Right'";
    public const string INVALID_POPUP_NAME_ERROR = "Popup name not valid";
    public const string MISSING_TEXT_GAMEOBJECT_ERROR = "MISSING_TEXT_GAMEOBJECT_ERROR";
    public const string INVALID_TEXT_COMPONENT_ERROR = "INVALID_TEXT_COMPONENT_ERROR";

    // UI METHOD UTILITIES (no errors)
    public const string SAME_IMAGE = "sameImage";
    public const string SAME_NAME = "sameName";
    public const string DEFAULT_TEXT = "Default text";
    public const string MISSING_GARMENTS = "missingGarments";
    public const string DUPLICATED_OUTFIT = "duplicatedOutfit";

    // UI FEEDBACK (no errors)
    public const string TOTAL_PAGES_SEE_MENU = "Totalpages on See Menu: ";
    public const string CURRENT_PAGE_SEE_MENU = "Current page: ";
    public const string RANDOM_PICKED_OUTFIT = "Random picked outfit: ";
    public const string SELECTED_OUTFIT_TO_EDIT = "Outfit selected to edit: ";
    public const string REMAINING_SLOTS_SEE_MENU = "Remaining Slots: ";

    // ENGINE ERRORS
    public const string EMPTY_LOCALIMAGEPATH_ERROR = "localImagePath is null or empty";
    public const string INVALID_TYPE_PARAMETER_ERROR = "Type parameter not valid loading image from explorer";
    public const string NULL_TARGET_IMAGE_ERROR = "Default target image is null";
    public const string NULL_TARGET_TEXT_ERROR = "Default target text is null";
    public const string INVALID_LOCAL_NAMES_OR_IMAGE_PATHS_ERROR = "Invalid or empty localNames or localImagePaths lists";
    public const string MISMATCHED_LENGS_LOCAL_LISTS_ERROR = "Mismatched lengths of localNames and localImagePaths lists";
    public const string INVALID_GARMENT_INDEX_ERROR = "Invalid garment index: ";
    public const string INVALID_LOCAL_IMAGE_PATH_ARRAY_ERROR = "Invalid or empty localImagePaths array";
    public const string NOT_FOUND_GARMENT_IMAGE_ERROR = "Garment image not found: ";
    public const string NO_GARMENT_TYPE_ERROR = "Garment has no type";

    // DATABASE ERRORS
    public const string GARMENT_VALIDATION_FAILED_ERROR = "Garment info validation failed";
    public const string OUTFIT_VALIDATION_FAILED_ERROR = "Outfit info validation failed";
    public const string GARMENT_INSERTION_FAILED_ERROR = "Garment insertion failed. Exception details: ";
    public const string OUTFIT_NOT_FOUND_TO_EDIT_ERROR = "Outfit to edit was not found on DB with id=";
    public const string OUTFIT_NOT_FOUND_TO_DELETE_ERROR = "Outfit to delete was not found on DB with id=";
    public const string NOT_FOUND_GARMENT_ERROR = "Garment not found in garmentsList with id=";

    // DATABASE FEEDBACK (no errors)
    public const string NO_OUTFITS_DB = "There are no outfits on database";
    public const string OUTFIT_SAVED = "Outfit saved successfully";
    public const string OUTFIT_FOUND_TO_EDIT = "Outfit to edit was found on DB with id=";

    //MISCELLANY (no errors)
    public const string ALL = "All";   // TODO: change to "all" and check everything works
    public const string TRUE = "true";
    public const string FALSE = "false";
}
