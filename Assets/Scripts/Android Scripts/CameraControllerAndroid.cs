using UnityEngine;
using UnityEngine.UI;
using NativeCameraNamespace;

public class CameraControllerAndroid : MonoBehaviour
{
    public Image imageToDisplay; // Referencia al componente Image donde se mostrará la imagen

    public void TakePictureFromCamera()
    {
     

        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            if (path != null) // Verificar si se tomó una foto
            {
                // Cargar la imagen tomada en una textura
                Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize: -1, markTextureNonReadable: true);
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
                Debug.LogWarning("Toma de foto cancelada");
            }
        }, maxSize: 1024);
    }
}
