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

    void Start()
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
            editShoesImage,
 
        };
        targetEditTexts = new List<GameObject>
        {
            editTopText,
            editBottomText,
            editShoesText,
        };

        targetDeleteImages = new List<Image>
        {
            deleteTopImage,
            deleteBottomImage,
            deleteShoesImage,

        };
        targetDeleteTexts = new List<GameObject>
        {
            deleteTopText,
            deleteBottomText,
            deleteShoesText,
        };
    }

    public void ProcessLocalData(List<string[]> localNames, List<string[]> localImagePaths)
    {
        if (localNames == null || localImagePaths == null || localNames.Count == 0 || localImagePaths.Count == 0)
        {
            Logger.Log(LogLevel.Error, "Invalid or empty localNames or localImagePaths lists.");
            return;
        }

        if (localNames.Count != localImagePaths.Count)
        {
            Logger.Log(LogLevel.Error, "Mismatched lengths of localNames and localImagePaths lists.");
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
                Logger.Log(LogLevel.Error, "Invalid garment index: " + i);
            }
        }
    }

    public void ProcessLocalData(string[] localNames, string[] localImagePaths)
    {

        if (localImagePaths == null || localImagePaths.Length == 0)
        {
            Logger.Log(LogLevel.Error, "Invalid or empty localImagePaths array.");
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
                }
                else
                {
                    Logger.Log(LogLevel.Error, "Garment image not found: " + localImagePath); // TODO: Warning or Error?
                }
            }
            else
            {
                Logger.Log(LogLevel.Error, "Invalid garment index: " + i);
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
        // Check if imagePath is not null
        if (string.IsNullOrEmpty(localImagePath))
        {
            Logger.Log(LogLevel.Error, "localImagePath is null or empty.");
            yield break;
        }
        // Check if imagePath exists
        else if (localImagePath.Equals(DatabaseManager.NO_IMAGE_PATH))
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
                    Logger.Log(LogLevel.Error, "Error loading texture: " + www.error);
                }
                else
                {
                    // Get the downloaded texture
                    Texture2D texture = DownloadHandlerTexture.GetContent(www);

                    // Create a sprite and assign it to the target image
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    targetImage.sprite = sprite;
                }
            }
        }

      
    }

    public void LoadImageFromExplorer(string type)
    {
        // Abrir el panel de selección de archivo
        string imagePath = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

        // Comprobar si se seleccionó un archivo
        if (!string.IsNullOrEmpty(imagePath))
        {
            if (type.Equals("Top"))
                StartCoroutine(LoadImageFromPath(createTopImage, imagePath));
            else if (type.Equals("Bottom"))
                StartCoroutine(LoadImageFromPath(createBottomImage, imagePath));
            else if (type.Equals("Shoes"))
                StartCoroutine(LoadImageFromPath(createShoesImage, imagePath));
            else
                Logger.Log(LogLevel.Error, "Type parameter not valid on LoadImageFromExplorer()");
        }
    }

    public void SetDefaultImageAndText(Image targetImage = null, List<Image> targetImages = null, GameObject targetText = null, List<GameObject> targetTexts = null)
    {
        // Verifica si se proporcionó una lista o un solo objeto para las imágenes
        if (targetImages != null)
        {
            targetImages.ForEach(img => SetDefaultImage(img));
        }
        else if (targetImage != null)
        {
            SetDefaultImage(targetImage);
        }

        // Verifica si se proporcionó una lista o un solo objeto para los textos
        if (targetTexts != null)
        {
            targetTexts.ForEach(txt => SetDefaultText(txt));
        }
        else if (targetText != null)
        {
            SetDefaultText(targetText);
        }
    }

    public void SetDefaultImage(Image targetImage)
    {
        if (targetImage != null)
        {
            targetImage.sprite = defaultSprite;
        }
        else
        {
            Logger.Log(LogLevel.Error, "Target image is null on SetDefaultImageAndText()");
        }
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
                targetTextComponent.text = "Default text";
            }
            else
            {
                Logger.Log(LogLevel.Error, "Text component not found in object: " + targetTextObject.name);
            }
        }
        else
        {
            Logger.Log(LogLevel.Error, "Target text GameObject is null at SetDefaultImageAndText()");
        }
    }
}
