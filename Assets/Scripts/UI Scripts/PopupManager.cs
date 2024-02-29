using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel;

    // Método para mostrar el popup con un mensaje
    public void ShowPopup(string message)
    {
        // Activar el panel
        popupPanel.SetActive(true);

        // Asignar el mensaje al texto del panel
        popupPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
    }

    public void HidePopup()
    {
        // Desactivar el panel
        popupPanel.SetActive(false);
    }
    public void ShowPopup()
    {
        // Desactivar el panel
        popupPanel.SetActive(true);
    }
}
