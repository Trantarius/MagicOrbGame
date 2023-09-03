using UnityEngine;

public class Pond : MonoBehaviour
{
    public AudioSource waterOrbEnterAudioSource;
    [SerializeField] private int orbsRequired;
    private int orbsCollected = 0;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "WaterOrb")
        {
            orbsCollected += 1;
            if (waterOrbEnterAudioSource != null) {
                waterOrbEnterAudioSource.Play();
            }

            Debug.Log(orbsCollected);
            Debug.Log("req: " + waterOrbEnterAudioSource != null);
            if (orbsCollected >= orbsRequired)
            {
                EventBus.RaiseOnLevelCompleted();
            }
        }
    }
}
