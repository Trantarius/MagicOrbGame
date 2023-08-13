using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryOrb : MonoBehaviour
{

    public float speed=10.0f;
    private Detector detector;
    private Rigidbody body;
    // Start is called before the first frame update
    void Start()
    {
        detector=transform.GetChild(0).GetComponent<Detector>();
        body=GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        List<GameObject> detected=detector.GetVisible();

        if(detected.Count==0){
            return;
        }

        Vector3 target=detected[0].transform.position;
        Vector3 to_target=(target-transform.position).normalized;

        Vector3 torque=Vector3.Cross(to_target,Vector3.down)*speed;
        body.AddTorque(torque);
    }
}
