using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBallBehavior : MonoBehaviour
{
    public float damage = 1f; // Adjust as needed

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Thief"))
        {
            collision.gameObject.SendMessage("HitByMagicBall", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
