using UnityEngine;

public class Timer : MonoBehaviour
{
    public float sessionDuration = 300f;
    private float timeRemaining;
    private bool sessionActive = false;

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
    }

    private void Update()
    {
        if (!sessionActive) return;

        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                timeRemaining = 0f;
                sessionActive = false;

                Debug.Log("Session Over");
                
                SaveSystem.Instance?.EndGame();
                
                GameObject gameOverPanel = GameObject.Find("GameOverPanel");
                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true);
                }
                
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (var enemy in enemies)
                {
                    Destroy(enemy);
                }
            }
        }
    }
}
