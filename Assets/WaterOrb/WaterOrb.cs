using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOrb : MonoBehaviour
{
    [Tooltip("Scales force used to respond to stimuli")]
    public float magneticForce = 10f;
    public AudioSource scaredAudioSource;

    private Rigidbody rb;
    private Detector detector;
    private bool hasSeenScaryOrb = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        detector = transform.GetChild(0).GetComponent<Detector>();
    }

    void FixedUpdate()
    {
        Vector3 moveDir=Vector3.zero;

        List<AttractionOrb> attractionOrbs = detector.GetNearbyComponents<AttractionOrb>();
        foreach(AttractionOrb orb in attractionOrbs){
            Vector3 rel=orb.transform.position-transform.position;
            float dist=rel.magnitude;
            //minimum distance prevents the water orb from pushing the attractor forever
            if(rel.magnitude>2){
                moveDir+=rel/dist;
            }
        }

        List<ScaryOrb> scaryOrbs = detector.GetVisibleComponent<ScaryOrb>();
        foreach(ScaryOrb orb in scaryOrbs){
            moveDir-=(orb.transform.position-transform.position).normalized;
        }

        Vector3 torq=Vector3.Cross(moveDir,Vector3.down);
        rb.AddTorque(torq*magneticForce);

        if (!hasSeenScaryOrb && scaryOrbs.Count > 0) {
            scaredAudioSource.Play();
            hasSeenScaryOrb = true;
        }
    }
}
