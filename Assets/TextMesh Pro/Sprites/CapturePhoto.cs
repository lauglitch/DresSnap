using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CapturePhoto : MonoBehaviour
{
    [SerializeField] private ARCameraManager arCameraManager;

    public void OnCaptureButtonClicked()
    {
        StartCoroutine(TakePhoto());
    }

    private IEnumerator TakePhoto()
    {
        yield return new WaitForEndOfFrame();

        Texture2D photo = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        photo.Apply();

        // Puedes hacer algo con la textura de la foto, como guardarla o mostrarla en tu interfaz de usuario.
        // Por ejemplo, puedes guardarla en el sistema de archivos del dispositivo o mostrarla en un objeto de imagen en tu aplicación.

        Destroy(photo); // Libera la textura para evitar fugas de memoria.
    }
}
