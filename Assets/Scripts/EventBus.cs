using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus singletonInstance;
    public delegate void OnLevelCompleted();
    public static event OnLevelCompleted onLevelCompleted;

   
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

    public static void RaiseOnLevelCompleted() {
        if (onLevelCompleted != null)
        {
            onLevelCompleted();
        }
    }
}
