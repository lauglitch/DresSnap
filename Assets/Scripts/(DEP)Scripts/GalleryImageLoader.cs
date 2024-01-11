using UnityEngine;
using UnityEngine.UI;


public class GalleryImageLoader : MonoBehaviour
{

    public Image imageToDisplay; // Referencia al componente Image donde se mostrar� la imagen

    public void LoadImageFromGallery()
    {
#if UNITY_ANDROID
        // Abrir la galer�a para seleccionar una imagen
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null) // Verificar si se seleccion� una imagen
            {
                // Cargar la imagen seleccionada en una textura
                Texture2D texture = NativeGallery.LoadImageAtPath(path);
                if (texture != null)
                {
                    // Crear un Sprite a partir de la textura
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                    // Asignar el Sprite al Image del UI para mostrar la imagen
                    imageToDisplay.sprite = sprite;
                }
                else
                {
                    Debug.LogError("Error al cargar la imagen");
                }
            }
            else
            {
                Debug.LogWarning("Selecci�n de imagen cancelada");
            }
        }, "Selecciona una imagen");
#else
        Debug.LogWarning("Esta funci�n est� disponible solo en dispositivos Android");
#endif
    }
}


