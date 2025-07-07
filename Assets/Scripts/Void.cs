using UnityEngine;

public class Void : MonoBehaviour
{
    public GameObject brokeTheGameMenu;
    public GameObject menuHUD;
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private PlayerProjectileCasting playerProjectileCasting;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;
            menuHUD.SetActive(false);
            if (playerMovement) playerMovement.isPaused = true;
            if (playerCamera) playerCamera.isPaused = true;
            if(playerProjectileCasting)  playerProjectileCasting.isPaused = true;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            if (brokeTheGameMenu != null)
            {
                brokeTheGameMenu.SetActive(true);
            }
            else
            {
                Debug.LogWarning("BrokeTheGameMenu is not assigned in the inspector.");
            }
        }
    }
}
