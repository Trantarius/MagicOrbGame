using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus singletonInstance;
    public delegate void OnLevelCompleted();
    public static event OnLevelCompleted onLevelCompleted;

    public delegate void OnLevelFailed();
    public static event OnLevelFailed onLevelFailed;

   
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

    public static void RaiseOnLevelCompleted()
    {
        if (onLevelCompleted != null)
        {
            onLevelCompleted();
        }
    }

    public static void RaiseOnLevelFailed()
    {
        if (onLevelFailed != null)
        {
            onLevelFailed();
        }
    }
}
