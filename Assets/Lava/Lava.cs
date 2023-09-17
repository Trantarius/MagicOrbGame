using UnityEngine;

public class Lava : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "WaterOrb")
        {
            EventBus.RaiseOnLevelFailed();
        }
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            EventBus.RaiseOnPlayerHit(1f);
        } 
    }
}
