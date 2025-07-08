using UnityEngine;

public class PickUpStats : MonoBehaviour
{
    public static PickUpStats Instance { get; private set; }

    public bool CollectedDoubleJump { get; private set; }
    public bool CollectedDash { get; private set; } // NEW

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDoubleJumpCollected()
    {
        CollectedDoubleJump = true;
    }

    public void SetDashCollected() // NEW
    {
        CollectedDash = true;
    }
    
    public void ResetStats()
    {
        CollectedDoubleJump = false;
        CollectedDash = false;
    }
}
