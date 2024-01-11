using UnityEngine;
using System.Collections;
using UnityEngine.Android; // Aseg�rate de importar el espacio de nombres de Android

public class PermissionManager : MonoBehaviour
{
    // M�todo para verificar y solicitar permisos de c�mara
    IEnumerator Start()
    {
        // Verificar si ya se han otorgado los permisos de c�mara
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            // Solicitar permisos de c�mara
            Permission.RequestUserPermission(Permission.Camera);

            // Esperar hasta que se otorguen los permisos
            yield return new WaitForSeconds(1);

            // Verificar si se otorgaron los permisos
            if (Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                // Los permisos de c�mara fueron otorgados
                Debug.Log("Camera permissions granted.");
            }
            else
            {
                // Los permisos de c�mara no fueron otorgados
                Debug.Log("Camera permissions not granted.");
            }
        }
        else
        {
            // Los permisos de c�mara ya han sido otorgados
            Debug.Log("Camera permits have already been granted.");
        }
    }
}
