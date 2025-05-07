using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition;
   
    void Update()
    {
        //Set camera position to the player
        transform.position = cameraPosition.position;     
    }
}
