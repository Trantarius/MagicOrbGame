using UnityEngine;

public class Lava : MonoBehaviour
{
    public HealthBar healthBar;

    void OnTriggerEnter(Collider col)
    {
        if ((col.gameObject.tag == "WaterOrb" || col.gameObject.tag == "Player") && healthBar.health != 1f)
        {
            healthBar.TakeDamage(1f);
            EventBus.RaiseOnLevelFailed();
        } 
        else if ((col.gameObject.tag == "WaterOrb" || col.gameObject.tag == "Player") && healthBar.health == 1f)
        {
            EventBus.RaiseOnGameFailed();
        }
    }
}
