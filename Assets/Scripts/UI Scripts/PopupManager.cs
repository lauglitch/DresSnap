using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject popupPanel;

    public void ShowPopup(string message)
    {
        popupPanel.SetActive(true);

        popupPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = message;
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
    public void ShowPopup()
    {
        popupPanel.SetActive(true);
    }
}
