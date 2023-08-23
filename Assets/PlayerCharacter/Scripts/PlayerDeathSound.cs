using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathSound : MonoBehaviour
{
    public AudioSource playerDeathAudioSource;

    public void PlayDeathSound()
    {
        playerDeathAudioSource.Play();
    }
}
