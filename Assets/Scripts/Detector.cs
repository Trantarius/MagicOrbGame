using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{

    [Tooltip("Identifies which layers should be detectable")]
    public LayerMask mask;

    [Tooltip("Identifies which layers should be able to block the line of sight")]
    public LayerMask rayMask;

    private List<GameObject> nearby=new List<GameObject>();

    private bool objectMatchesMask(GameObject obj){
        return ((1<<obj.layer) & mask.value) != 0;
    }

    void OnTriggerEnter(Collider other){
        if(objectMatchesMask(other.gameObject) && !nearby.Contains(other.gameObject)){
            nearby.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other){
        if(objectMatchesMask(other.gameObject) && nearby.Contains(other.gameObject)){
            nearby.Remove(other.gameObject);
        }
    }

    // Gets all detected objects, regardless of visibility.
    public List<GameObject> getNearby(){
        return new List<GameObject>(nearby);
    }

    // Gets detected instances of a particular component type.
    public List<T> getNearbyComponent<T>(){
        List<T> ret=new List<T>();

        foreach(GameObject nb in nearby){
            T comp;
            if(nb.TryGetComponent<T>(out comp)){
                ret.Add(comp);
            }
        }
        return ret;
    }

    // Gets only the objects that are visible to this detector.
    public List<GameObject> getVisible(){
        List<GameObject> ret=new List<GameObject>();

        foreach(GameObject nb in nearby){

            RaycastHit rayhit=new RaycastHit();
            Vector3 dir=(nb.transform.position-transform.position);
            float dirlen=dir.magnitude;
            dir/=dirlen;

            bool hit = Physics.Raycast(transform.position, dir, out rayhit,
                dirlen, rayMask.value, QueryTriggerInteraction.Ignore);
            
            if(!(hit && rayhit.collider.gameObject!=nb)){
                ret.Add(nb);
            }
        }
        return ret;
    }

    // Gets only the T instances that are visible to this detector.
    public List<T> getVisibleComponent<T>(){
        List<T> ret=new List<T>();

        foreach(GameObject nb in nearby){
            T comp;
            if(nb.TryGetComponent<T>(out comp)){

                RaycastHit rayhit=new RaycastHit();
                Vector3 dir=(nb.transform.position-transform.position);
                float dirlen=dir.magnitude;
                dir/=dirlen;

                bool hit = Physics.Raycast(transform.position, dir, out rayhit,
                    dirlen, rayMask.value, QueryTriggerInteraction.Ignore);
                
                if(!(hit && rayhit.collider.gameObject!=nb)){
                    ret.Add(comp);
                }
            }
        }
        return ret;
    }
}
