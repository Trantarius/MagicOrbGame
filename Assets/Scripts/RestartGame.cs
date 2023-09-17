using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public void onRestartGame()
    {
        EventBus.RaiseOnGameRestarted();
    }
}
