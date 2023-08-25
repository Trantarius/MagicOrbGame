using UnityEngine;

public class PlayerDeathSound : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.onLevelFailed += PlayDeathSound;
    }

    void OnDisable()
    {
        EventBus.onLevelFailed -= PlayDeathSound;
    }

    public AudioSource playerDeathAudioSource;

    public void PlayDeathSound()
    {
        playerDeathAudioSource.Play();
    }
}
