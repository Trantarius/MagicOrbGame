using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus singletonInstance;
    public delegate void OnLevelCompleted();
    public static event OnLevelCompleted onLevelCompleted;

    public delegate void OnLevelFailed();
    public static event OnLevelFailed onLevelFailed;

    public delegate void OnGameFailed();
    public static event OnGameFailed onGameFailed;
    public delegate void OnGameRestarted();
    public static event OnGameRestarted onGameRestarted;
    public delegate void OnGameStarted();
    public static event OnGameStarted onGameStarted;

   
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

    public static void RaiseOnGameFailed()
    {
        if (onGameFailed != null)
        {
            onGameFailed();
        }
    }

    public static void RaiseOnGameRestarted()
    {
        if (onGameRestarted != null)
        {
            onGameRestarted();
        }
    }

    public static void RaiseOnGameStarted()
    {
        if (onGameStarted != null)
        {
            onGameStarted();
        }
    }
}
