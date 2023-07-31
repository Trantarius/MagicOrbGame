using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Tooltip("The object to follow")]
    public GameObject target;
    [Tooltip("Offset between camera and player")]
    public Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation=Quaternion.identity;
        transform.position = target.transform.position+offset;
    }
}