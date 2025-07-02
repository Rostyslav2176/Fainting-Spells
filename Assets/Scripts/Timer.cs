using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float sessionDuration = 120f;
    private float timeRemaining;
    private bool sessionActive = false;
    
    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private PlayerProjectileCasting playerProjectileCasting;
    
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;

    public static Timer Instance { get; private set; }

    public float TimeRemaining => timeRemaining;
    public float ElapsedTime => sessionDuration - timeRemaining;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        timeRemaining = sessionDuration;
        sessionActive = true;
        
        playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        playerCamera = Object.FindFirstObjectByType<PlayerCamera>();
        playerProjectileCasting = Object.FindFirstObjectByType<PlayerProjectileCasting>();
    }

    private void Update()
    {
        if (!sessionActive) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining < 0f)
            {
                timeRemaining = 0f;
            }

            UpdateTimerUI();

            if (timeRemaining == 0f)
            {
                sessionActive = false;

                Debug.Log("Session Over");

                SaveSystem.Instance?.EndGame();
                
                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true);
                }
                
                if (playerMovement) playerMovement.isPaused = true;
                if (playerCamera) playerCamera.isPaused = true;
                if(playerProjectileCasting)  playerProjectileCasting.isPaused = true;
            }
        }
    }
    
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = seconds.ToString();
        }
    }
}
