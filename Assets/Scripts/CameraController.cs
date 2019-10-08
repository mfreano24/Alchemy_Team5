using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //TODO: Zoom the camera out and also create buttons to move the camera further along
    public Transform target;
    public float dampTime; //Make the camera hesitate before following
    private Vector3 velocity = Vector3.zero;
    


//LateUpdate is called at the last possible frame.
    void LateUpdate(){
        if(target){ // if the target exists (I guess this returns a boolean?)
            Vector3 camera_position = GetComponent<Camera>().WorldToViewportPoint(target.position);
            //Moves the camera a little farther back so that it's not ON the player.
            Vector3 target_position = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f,0.5f,camera_position.z));
            Vector3 destination = transform.position + target_position;
            //move smoothly, bud
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
            LookCheck();
        
            
            
        }
    }

    void LookCheck(){ //The directional bias Tim mentioned?
        if(Camera.main.orthographicSize < 20f){
            if(Input.GetKey(KeyCode.E))
            {
                Camera.main.orthographicSize += 0.2f;
            }
            if(Camera.main.orthographicSize > 14f && !Input.GetKey(KeyCode.E))
            {
                 Camera.main.orthographicSize -= 0.2f;
            }
        }
        else if(Camera.main.orthographicSize >= 20f)
        {
            if(Camera.main.orthographicSize > 14f && !Input.GetKey(KeyCode.E))
            {
                Camera.main.orthographicSize -= 0.2f;
            }
        }
    }
}
