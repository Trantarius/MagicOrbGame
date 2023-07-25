using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScaryOrbTestCamera : MonoBehaviour
{
    public Transform following;
    public Vector3 offset;

    public Transform fakePlayer;

    private bool isPlayerActive=false;

    void Start(){
        fakePlayer.position=new Vector3(1000_000,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position=following.position+offset;
        if(isPlayerActive){
            fakePlayer.position=getMousePosition();
        }
        else{
            fakePlayer.position=new Vector3(1000_000,0,0);
        }
    }

    private Vector3 getMousePosition(){
        Vector2 screen=Mouse.current.position.ReadValue();
        Ray ray=Camera.main.ScreenPointToRay(new Vector3(screen.x,screen.y,0));
        return ray.origin + ray.direction * 
            Mathf.Abs(ray.origin.z / Vector3.Dot(ray.direction,Vector3.forward));
        
    }

    public void OnClick(InputAction.CallbackContext context){
        isPlayerActive=context.performed;
    }
}
