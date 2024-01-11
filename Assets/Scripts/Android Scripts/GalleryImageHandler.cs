using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GalleryImageHandler : MonoBehaviour
{
    public Image imageToDisplay; // El objeto Image de Unity donde se mostrará la imagen
    private Garment garment; // Objeto que representa la tabla GarmentDB en la base de datos

    void Start()
    {
        garment = new Garment(); // Acceder o crear la instancia de la tabla GarmentDB en tu base de datos SQLite
        string savedImagePath = garment.ImagePath; // Obtener la ruta de la imagen desde la tabla GarmentDB

        if (!string.IsNullOrEmpty(savedImagePath))
        {
            LoadImage(savedImagePath); // Cargar la imagen desde la ruta obtenida
        }
    }

    public void SelectImageFromGallery()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                LoadImage(path);
                garment.ImagePath = path; // Guardar la ruta de la imagen en el atributo ImagePath de GarmentDB en la base de datos
                garment.InsertGarmentDB(garment); // Guardar los cambios en la base de datos SQLite
            }
        }, "Selecciona una imagen");
    }

    void LoadImage(string path)
    {
        // Aquí cargarías la imagen utilizando la ruta proporcionada (path)
        // Asegúrate de que path sea una URL válida o una ruta de archivo local que pueda ser cargada como textura

        // Ejemplo básico: cargar la textura desde la ruta (ejemplo para URL)
        StartCoroutine(LoadTextureFromURL(path));
    }

    IEnumerator LoadTextureFromURL(string url)
    {
        using (WWW www = new WWW(url))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                Texture2D texture = www.texture;
                if (texture != null)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    imageToDisplay.sprite = sprite;
                }
            }
            else
            {
                Debug.LogError("Error al cargar la imagen desde la URL: " + url + "\nError: " + www.error);
            }
        }
    }
}
