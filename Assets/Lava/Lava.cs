using UnityEngine;

public class Lava : MonoBehaviour
{
    public PlayerDeathSound playerDeathSound;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "WaterOrb" || col.gameObject.tag == "Player")
        {
            playerDeathSound.PlayDeathSound();
            EventBus.RaiseOnLevelFailed();
        }
    }
}
