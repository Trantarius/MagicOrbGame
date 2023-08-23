using UnityEngine;
using UnityEngine.UI;

public class TextMessages : MonoBehaviour
{
    [SerializeField] 
    public Text textMessage;

    public static TextMessages singletonInstance;
    void Awake()
    {
        if (singletonInstance == null)
        {
            singletonInstance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void OnEnable()
    {
        EventBus.onLevelCompleted += ShowLevelCompletedText;
        EventBus.onLevelFailed += ShowLevelFailedText;
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= ShowLevelCompletedText;
        EventBus.onLevelFailed -= ShowLevelFailedText;
    }

    public void ShowLevelCompletedText()
    {
        textMessage.text = "You Won :D";
    }

    public void ShowLevelFailedText()
    {
        textMessage.text = "You Failed :'(";
    }
}
