using UnityEngine;
using System.Collections;
using UnityEngine.Android; // Asegúrate de importar el espacio de nombres de Android

public class PermissionManager : MonoBehaviour
{
    // Método para verificar y solicitar permisos de cámara
    IEnumerator Start()
    {
        // Verificar si ya se han otorgado los permisos de cámara
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // Solicitar permisos de cámara
            Permission.RequestUserPermission(Permission.Camera);

            // Esperar hasta que se otorguen los permisos
            yield return new WaitForSeconds(1);

            // Verificar si se otorgaron los permisos
            if (Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                // Los permisos de cámara fueron otorgados
                Logger.Log(LogLevel.Success, "Camera permissions granted.");
            }
            else
            {
                // Los permisos de cámara no fueron otorgados
                Logger.Log(LogLevel.Warning, "Camera permissions not granted.");
            }
        }
        else
        {
            // Los permisos de cámara ya han sido otorgados
            Logger.Log(LogLevel.Success, "Camera permits have already been granted.");
        }
    }
}
