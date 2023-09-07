using UnityEngine;

public class LivesBarHolder : MonoBehaviour
{
    public Transform player;
    
    void Update()
    {
        Vector3 offset = new Vector3(0f, 0.1f, -1f);
        transform.position = player.position + offset;
    }
}
