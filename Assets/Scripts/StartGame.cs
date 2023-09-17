using UnityEngine;

public class StartGame : MonoBehaviour
{
    public void onStartGame()
    {
        EventBus.RaiseOnGameStarted();
    }
}
