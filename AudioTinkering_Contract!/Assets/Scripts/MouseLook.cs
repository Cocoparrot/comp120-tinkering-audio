using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    /*
     * Starting variables
     * MouseSensitivity set so it is the same for everyone at the start and we can use it to make the movements independent to the framerate when used.
     * Declaring the playerbody we need to add so the program knows what we are rotating. 
     * 
     */

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        //lock the cursor so it stays in the game screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //with the use of the charactercontroller component we can use the input system to get the movement of the mouse ingame without having to declare a lot of variables that calculate the axises
        //multiply the movement of the mouse with the sensitivity and Time.deltatime so it is not sensitive to framerate and the movement feels smooth.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Rotating on the Y axis. And clamping the rotation so we don't break our neck while playing.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotation on the x axis.
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
       
    }
}
