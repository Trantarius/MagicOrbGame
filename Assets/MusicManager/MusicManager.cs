using UnityEngine;
 
 public class MusicManager : MonoBehaviour
 {
    public static MusicManager singletonInstance;
     
    void Awake()
    {
        if (singletonInstance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            singletonInstance = this;
            var audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
        else
        {
            Destroy(this);
        }
    }
 }