using UnityEngine;

public class Pond : MonoBehaviour
{
    [SerializeField] private int orbsRequired;
    private int orbsCollected = 0;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "WaterOrb")
        {
            orbsCollected += 1;

            if (orbsCollected >= orbsRequired)
            {
                EventBus.RaiseOnLevelCompleted();
            }
        }
    }
}
