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
    }

    void OnDisable()
    {
        EventBus.onLevelCompleted -= ShowLevelCompletedText;
        EventBus.onLevelFailed -= ShowLevelFailedText;
    }

    public void ShowLevelCompletedText()
    {
        panel.SetActive(true);
        textMessage.text = "You Won!";
    }

    public void ShowLevelFailedText()
    {
        panel.SetActive(true);
        textMessage.text = "You Failed!";
    }
}
