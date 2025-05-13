using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float senX;
    public float senY;

    public Transform orientation;
    public Transform playerBody;
    
    float rotX;
    float rotY;
    void Start()
    {
        //Lock cursor in the middle of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        //Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * senY;
        
        rotY += mouseX;
        rotX -= mouseY;
        
        //Player can lock up and down only for 90 degrees
        rotX = Mathf.Clamp(rotX, -90f, 90f);
        
        //Rotate camera and orientation
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.localRotation = Quaternion.Euler(0, rotY, 0);
    }
}
