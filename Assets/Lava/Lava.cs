using UnityEngine;

public class Lava : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "WaterOrb" || col.gameObject.tag == "Player")
        {   
            EventBus.RaiseOnLevelFailed();
        }
    }
}
