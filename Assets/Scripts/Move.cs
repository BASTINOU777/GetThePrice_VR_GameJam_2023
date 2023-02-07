using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
     private float speed = 5.0f;
     private float turnSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Left Joystick
        float horizontalLeftJoy = Input.GetAxis("XRI_Left_Primary2DAxis_Horizontal");
        float verticalLeftJoy = Input.GetAxis("XRI_Left_Primary2DAxis_Vertical");
        //Right Joystick
        float horizontalRightJoy = Input.GetAxis("XRI_Right_Primary2DAxis_Horizontal");
        float verticalRightJoy = Input.GetAxis("XRI_Right_Primary2DAxis_Vertical");

        if(horizontalLeftJoy != 0 || verticalLeftJoy != 0){
            transform.position += transform.forward * verticalLeftJoy * speed * Time.deltaTime;
            transform.Rotate(Vector3.up, horizontalLeftJoy * turnSpeed * Time.deltaTime);
        }
        
    }
}
