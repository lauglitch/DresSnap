using UnityEngine;
using UnityEngine.UI;

public class AndroidLoggerText : MonoBehaviour
{
    public Text myText;

    private void Awake()
    {
        myText = GetComponent<Text>();
        AndroidLogger.SubscribeText(myText);
    }

    private void OnDestroy()
    {
        AndroidLogger.UnsubscribeText(myText);
    }
}
