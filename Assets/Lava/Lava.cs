using UnityEngine;

public class Lava : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        Debug.Log("1");
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "WaterOrb" || col.gameObject.tag == "Player")
        {
            Debug.Log("2");
            EventBus.RaiseOnLevelFailed();
        }
    }
}
