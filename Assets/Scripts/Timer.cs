using UnityEngine;
using TMPro;
using System.Linq;

public class Timer : MonoBehaviour
{
    [Header("Session Settings")]
    public float sessionDuration = 120f;

    [Header("UI Elements")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI doubleJumpText;
    public TextMeshProUGUI dashText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI skullKillText;
    public TextMeshProUGUI eyeKillText;
    public TextMeshProUGUI crystalKillText;

    public GameObject gameOverPanel;

    private float timeRemaining;
    private bool sessionActive = false;

    private PlayerMovement playerMovement;
    private PlayerCamera playerCamera;
    private PlayerProjectileCasting playerProjectileCasting;

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

        PickUpStats.Instance?.ResetStats();
        EnemyKillTracker.Instance?.ResetKills();
    }

    private void Update()
    {
        if (!sessionActive) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0f)
                timeRemaining = 0f;

            UpdateTimerUI();

            if (timeRemaining == 0f)
            {
                EndSession();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = $"{seconds}s";
        }
    }

    private void EndSession()
    {
        sessionActive = false;
        Time.timeScale = 0f;

        Debug.Log("Session Over");

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (doubleJumpText != null)
            {
                bool hasDoubleJump = PickUpStats.Instance?.CollectedDoubleJump ?? false;
                doubleJumpText.text = hasDoubleJump ? "Double Jump: Collected" : "Double Jump: Not Collected";
            }

            if (dashText != null)
            {
                bool hasDash = PickUpStats.Instance?.CollectedDash ?? false;
                dashText.text = hasDash ? "Dash: Collected" : "Dash: Not Collected";
            }

            if (skullKillText != null)
            {
                int skullKills = EnemyKillTracker.Instance?.GetKillCount("A") ?? 0;
                skullKillText.text = $"Skull Enemies Killed: {skullKills}";
            }

            if (eyeKillText != null)
            {
                int eyeKills = EnemyKillTracker.Instance?.GetKillCount("B") ?? 0;
                eyeKillText.text = $"Eye Enemies Killed: {eyeKills}";
            }

            if (crystalKillText != null)
            {
                int crystalKills = EnemyKillTracker.Instance?.GetKillCount("C") ?? 0;
                crystalKillText.text = $"Crystal Enemies Killed: {crystalKills}";
            }

            if (killCountText != null)
            {
                int totalKills = EnemyKillTracker.Instance?.GetAllKills().Values.Sum() ?? 0;
                killCountText.text = $"Total Enemies Killed: {totalKills}";
            }
        }

        if (playerMovement) playerMovement.isPaused = true;
        if (playerCamera) playerCamera.isPaused = true;
        if (playerProjectileCasting) playerProjectileCasting.isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
