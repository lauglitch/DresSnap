using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using UnityEditor;
using System.IO;
using TMPro;

public class GalleryLoader : MonoBehaviour
{
    public Sprite defaultSprite;

    public Image createTopImage;
    public Image createBottomImage;
    public Image createShoesImage;

    public Image pickTopImage;
    public Image pickBottomImage;
    public Image pickShoesImage;
    public GameObject pickTopText;
    public GameObject pickBottomText;
    public GameObject pickShoesText;

    public Image seeTopImage1;
    public Image seeBottomImage1;
    public Image seeShoesImage1;
    public GameObject seeTopText1;
    public GameObject seeBottomText1;
    public GameObject seeShoesText1;
    public Image seeTopImage2;
    public Image seeBottomImage2;
    public Image seeShoesImage2;
    public GameObject seeTopText2;
    public GameObject seeBottomText2;
    public GameObject seeShoesText2;
    public Image seeTopImage3;
    public Image seeBottomImage3;
    public Image seeShoesImage3;
    public GameObject seeTopText3;
    public GameObject seeBottomText3;
    public GameObject seeShoesText3;

    public Image editTopImage;
    public Image editBottomImage;
    public Image editShoesImage;
    public GameObject editTopText;
    public GameObject editBottomText;
    public GameObject editShoesText;

    public Image deleteTopImage;
    public Image deleteBottomImage;
    public Image deleteShoesImage;
    public GameObject deleteTopText;
    public GameObject deleteBottomText;
    public GameObject deleteShoesText;

    public List<Image> targetPickImages;        // Refers only to 3 images displayed in the Pick Outfit Popup.
    public List<GameObject> targetPickTexts;    // Refers only to 3 images displayed in the Pick Outfit Popup.
    public List<Image> targetCreateImages;      // Refers only to 3 images displayed in the Create Outfit Menu.
    public List<Image> targetSeeImages;         // Refers only to 9 images displayed in the Create Outfit Menu.
    public List<GameObject> targetSeeTexts;     // Refers only to 9 images displayed in the Create Outfit Menu.
    public List<Image> targetEditImages;        // Refers only to 3 images displayed in the Edit Popup.
    public List<GameObject> targetEditTexts;    // Refers only to 3 images displayed in the Edit Popup.
    public List<Image> targetDeleteImages;        // Refers only to 3 images displayed in the Delete Popup.
    public List<GameObject> targetDeleteTexts;    // Refers only to 3 images displayed in the Delete Menu Popup.

    // TODO: intentar eliminar esto
    public string createTopImagePath;
    public string createBottomImagePath;
    public string createShoesImagePath;

    private static GalleryLoader _instance;

    public static GalleryLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GalleryLoader>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<GalleryLoader>();
                    singleton.name = typeof(GalleryLoader).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
            SetTargetImages();
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetTargetImages()
    {

        targetPickImages = new List<Image>
        {
            pickTopImage,
            pickBottomImage,
            pickShoesImage
        };
        targetPickTexts = new List<GameObject>
        {
            pickTopText,
            pickBottomText,
            pickShoesText
        };

        targetCreateImages = new List<Image>
        {
            createTopImage,
            createBottomImage,
            createShoesImage
        };

        targetSeeImages = new List<Image>
        {
            seeTopImage1,
            seeBottomImage1,
            seeShoesImage1,
            seeTopImage2,
            seeBottomImage2,
            seeShoesImage2,
            seeTopImage3,
            seeBottomImage3,
            seeShoesImage3
        };
        targetSeeTexts = new List<GameObject>
        {
            seeTopText1,
            seeBottomText1,
            seeShoesText1,
            seeTopText2,
            seeBottomText2,
            seeShoesText2,
            seeTopText3,
            seeBottomText3,
            seeShoesText3
        };

        targetEditImages = new List<Image>
        {
            editTopImage,
            editBottomImage,
            editShoesImage
        };
        targetEditTexts = new List<GameObject>
        {
            editTopText,
            editBottomText,
            editShoesText
        };

        targetDeleteImages = new List<Image>
        {
            deleteTopImage,
            deleteBottomImage,
            deleteShoesImage
        };
        targetDeleteTexts = new List<GameObject>
        {
            deleteTopText,
            deleteBottomText,
            deleteShoesText
        };

        // Añadir logs para verificar asignaciones
        /*
        Debug.Log("Target Pick Images: " + targetPickImages.Count);
        targetPickImages.ForEach(img => Debug.Log(img != null ? img.name : "null"));

        Debug.Log("Target Pick Texts: " + targetPickTexts.Count);
        targetPickTexts.ForEach(txt => Debug.Log(txt != null ? txt.name : "null"));

        Debug.Log("Target Create Images: " + targetCreateImages.Count);
        targetCreateImages.ForEach(img => Debug.Log(img != null ? img.name : "null"));

        Debug.Log("Target See Images: " + targetSeeImages.Count);
        targetSeeImages.ForEach(img => Debug.Log(img != null ? img.name : "null"));

        Debug.Log("Target See Texts: " + targetSeeTexts.Count);
        targetSeeTexts.ForEach(txt => Debug.Log(txt != null ? txt.name : "null"));

        Debug.Log("Target Edit Images: " + targetEditImages.Count);
        targetEditImages.ForEach(img => Debug.Log(img != null ? img.name : "null"));

        Debug.Log("Target Edit Texts: " + targetEditTexts.Count);
        targetEditTexts.ForEach(txt => Debug.Log(txt != null ? txt.name : "null"));

        Debug.Log("Target Delete Images: " + targetDeleteImages.Count);
        targetDeleteImages.ForEach(img => Debug.Log(img != null ? img.name : "null"));

        Debug.Log("Target Delete Texts: " + targetDeleteTexts.Count);
        targetDeleteTexts.ForEach(txt => Debug.Log(txt != null ? txt.name : "null"));
        */
    }

    public void ProcessLocalData(List<string[]> localNames, List<string[]> localImagePaths)
    {
        if (localNames == null || localImagePaths == null || localNames.Count == 0 || localImagePaths.Count == 0)
        {
            Logger.Log(LogLevel.Error, Constants.INVALID_LOCAL_NAMES_OR_IMAGE_PATHS_ERROR);
            return;
        }

        if (localNames.Count != localImagePaths.Count)
        {
            Logger.Log(LogLevel.Error, Constants.MISMATCHED_LENGS_LOCAL_LISTS_ERROR);
            return;
        }

        List<Image> targetImages = new List<Image>
        {
            pickTopImage,
            pickBottomImage,
            pickShoesImage
        };

        for (int i = 0; i < localNames.Count; i++)
        {
            Image targetImage = GetImageByIndex(i, targetImages);
            if (targetImage != null)
            {
                // Assuming each list has at least one element
                string localImagePath = localImagePaths[i][0]; // Use the first element of the route list

                // Load its image on GameObject Image
                StartCoroutine(LoadImageFromPath(targetImage, localImagePath));
            }
            else
            {
                Logger.Log(LogLevel.Error, Constants.INVALID_GARMENT_INDEX_ERROR + i);
            }
        }
    }

    public void ProcessLocalData(string[] localNames, string[] localImagePaths)
    {

        if (localImagePaths == null || localImagePaths.Length == 0)
        {
            Logger.Log(LogLevel.Error, Constants.INVALID_LOCAL_IMAGE_PATH_ARRAY_ERROR);
            return;
        }

        for (int i = 0; i < localImagePaths.Length; i++)
        {
            Image targetImage = GetImageByIndex(i, targetPickImages);
            if (targetImage != null)
            {
                // Assuming each list has at least one element
                string localImagePath = localImagePaths[i];

                // Check if file exists
                if (File.Exists(localImagePath))
                {
                    // Load its image on GameObject Image
                    StartCoroutine(LoadImageFromPath(targetImage, localImagePath));

                    // Asignar el texto al GameObject de texto hijo
                    GameObject textObject = targetImage.transform.GetChild(0).gameObject; // Suponiendo que el texto es el primer hijo
                    if (textObject != null)
                    {
                        TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
                        if (textComponent != null && i < localNames.Length)
                        {
                            textComponent.text = localNames[i];
                        }
                        else
                        {
                            Logger.Log(LogLevel.Warning, Constants.INVALID_TEXT_COMPONENT_ERROR);
                        }
                    }
                    else
                    {
                        Logger.Log(LogLevel.Warning, Constants.MISSING_TEXT_GAMEOBJECT_ERROR);
                    }
                }
                else
                {
                    Logger.Log(LogLevel.Error, Constants.NOT_FOUND_GARMENT_IMAGE_ERROR + localImagePath); // TODO: Warning or Error?
                }
            }
            else
            {
                Logger.Log(LogLevel.Error, Constants.INVALID_GARMENT_INDEX_ERROR + i);
            }
        }


    }

    private Image GetImageByIndex(int index, List<Image> targetImages)
    {
        if (index >= 0 && index < targetImages.Count)
        {
            return targetImages[index];
        }
        return null;
    }

    public IEnumerator LoadImageFromPath(Image targetImage, string localImagePath)
    {
        Logger.Log(LogLevel.DeepTest, targetImage.name);

        // Check if imagePath is not null
        if (string.IsNullOrEmpty(localImagePath))
        {
            Logger.Log(LogLevel.Error, Constants.EMPTY_LOCALIMAGEPATH_ERROR);
            yield break;
        }
        // Check if imagePath exists
        else if (localImagePath.Equals(Constants.NO_IMAGE_PATH))
        {
            // Asegúrate de que SetDefaultImageAndText coincida con los tipos de datos esperados
            SetDefaultImageAndText(targetImages: new List<Image> { targetImage }, targetTexts: new List<GameObject> { /* Pasa aquí el GameObject que necesitas */ });
        }
        else
        {
            // Use UnityWebRequestTexture to load the texture asynchronously
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + localImagePath))
            {
                // Send the request and wait for it to complete
                yield return www.SendWebRequest();

                // Check for errors
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Logger.Log(LogLevel.Error, Constants.LOAD_IMAGE_ERROR + www.error);
                }
                else
                {
                    // Get the downloaded texture
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);

                    // Create a sprite and assign it to the target image
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                    targetImage.sprite = sprite;

                    if (targetImage == createTopImage)
                        createTopImagePath = localImagePath;
                    else if (targetImage == createBottomImage)
                        createBottomImagePath = localImagePath;
                    else if (targetImage == createShoesImage)
                        createShoesImagePath = localImagePath;

                }
            }
        }

      
    }

    public string LoadImageFromExplorer(string type)
    {
        // Abrir el panel de selección de archivo
        string imagePath = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

        // Comprobar si se seleccionó un archivo
        if (!string.IsNullOrEmpty(imagePath))
        {
            //Logger.Log(LogLevel.DeepTest, "imagePath: " + imagePath);

            if (type.Equals("Top"))
                StartCoroutine(LoadImageFromPath(createTopImage, imagePath));
            else if (type.Equals("Bottom"))
                StartCoroutine(LoadImageFromPath(createBottomImage, imagePath));
            else if (type.Equals("Shoes"))
                StartCoroutine(LoadImageFromPath(createShoesImage, imagePath));
            else
                Logger.Log(LogLevel.Error, Constants.INVALID_TYPE_PARAMETER_ERROR);

            return imagePath;
        }

        return Constants.UNDEFINED;
    }

    public void SetDefaultImageAndText(Image targetImage = null, List<Image> targetImages = null, GameObject targetText = null, List<GameObject> targetTexts = null)
    {
        // Lista para acumular los nombres de las prendas que se han seteado
        List<string> debugSettedGarments = new List<string>();

        // Set default image for one or more targetImage/s
        if (targetImages != null && targetImages.Count > 0)
        {
            targetImages.ForEach(img =>
            {
                if (img != null)
                {
                    SetDefaultImage(img);
                    debugSettedGarments.Add(img.name);
                }
                else
                {
                    Logger.Log(LogLevel.Error, "One of the target images is null");
                }
            });
        }
        else if (targetImage != null)
        {
            SetDefaultImage(targetImage);
            debugSettedGarments.Add(targetImage.name);
        }
        else
        {
            Logger.Log(LogLevel.Error, "No target images provided");
        }

        // Set default text for one or more targetText/s
        if (targetTexts != null && targetTexts.Count > 0)
        {
            targetTexts.ForEach(txt =>
            {
                if (txt != null)
                {
                    SetDefaultText(txt);
                    debugSettedGarments.Add(txt.name);
                }
                else
                {
                    Logger.Log(LogLevel.Error, "One of the target texts is null");
                }
            });
        }
        else if (targetText != null)
        {
            SetDefaultText(targetText);
            debugSettedGarments.Add(targetText.name);
        }
        // No else case needed for missing texts.

        // Imprimir los elementos seteados en una sola línea
        if (debugSettedGarments.Count > 0)
        {
            Logger.Log(LogLevel.Test, ($"Setting default image for: {string.Join(", ", debugSettedGarments)}"));
        }
    }

    public void SetDefaultImage(Image targetImage)
    {
        if (targetImage == null)
        {
            Logger.Log(LogLevel.Error, Constants.NULL_TARGET_IMAGE_ERROR);
            return; // Salir del método si targetImage es null
        }

        // Asignar el sprite predeterminado a targetImage
        targetImage.sprite = defaultSprite;
    }

    private void SetDefaultText(GameObject targetTextObject)
    {
        // Verifica si el GameObject es nulo
        if (targetTextObject != null)
        {
            // Intenta obtener el componente de texto en el GameObject
            TMP_Text targetTextComponent = targetTextObject.GetComponent<TMP_Text>();

            // Verifica si el componente de texto es nulo
            if (targetTextComponent != null)
            {
                // Asigna el texto predeterminado al componente de texto
                targetTextComponent.text = Constants.DEFAULT_TEXT;
            }
            else
            {
                Logger.Log(LogLevel.Error, Constants.NOT_FOUND_TEXT_COMPONENT_ERROR + targetTextObject.name);
            }
        }
        else
        {
            Logger.Log(LogLevel.Error, Constants.NULL_TARGET_TEXT_ERROR);
        }
    }
}
