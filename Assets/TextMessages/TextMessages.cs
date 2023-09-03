using UnityEngine;
using UnityEngine.UI;

public class TextMessages : MonoBehaviour
{
    [SerializeField] 
    public Text textMessage;
    [SerializeField]
    public GameObject panel;

    public static TextMessages singletonInstance;
    void Awake()
    {
        if (singletonInstance == null)
        {
            singletonInstance = this;
            panel.SetActive(false);
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
        EventBus.onGameFailed += ShowGameFailedText;
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= ShowLevelCompletedText;
        EventBus.onLevelFailed -= ShowLevelFailedText;
        EventBus.onGameFailed -= ShowGameFailedText;
    }

    public void ShowLevelCompletedText()
    {
        panel.SetActive(true);
        textMessage.text = "Level completed!";
    }

    public void ShowLevelFailedText()
    {
        panel.SetActive(true);
        textMessage.text = "Level Failed!";
    }

    public void ShowGameFailedText()
    {
        panel.SetActive(true);
        textMessage.text = "Game Failed!";
    }
}
