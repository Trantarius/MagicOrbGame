using UnityEngine;

public class VolumeControlHolder : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        Vector3 offset = new Vector3(-13f, 13f, -1f);
        transform.position = player.position + offset;
    }
}
