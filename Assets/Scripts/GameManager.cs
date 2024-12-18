using UnityEngine;
using TMPro;
using UnityEngine.UI; // Import TextMeshPro namespace

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public float timeRemaining = 180f; // Default 3 minutes
    public TextMeshProUGUI timerText; // Reference to TextMeshProUGUI
    public TextMeshProUGUI zombieKillText;
    public Slider healthSlider;
    // public TextMeshProUGUI scoreText; // Updated for score display as well
    // public GameObject gameOverPanel;


    private int zombieKillCount = 0; // Tracks zombie kills
    private bool isGameOver = false;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        zombieKillCount = 0;
        UpdateZombieKillUI();
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; // Set max value of slider
            healthSlider.value = currentHealth; // Update the slider value
        }
    }

    void Update()
    {
        if (isGameOver) return;

        // Timer Countdown
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(0, timeRemaining); // Clamp to zero to avoid negatives
            UpdateTimerUI();
        }
        else
        {
            EndGame();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}"; // Update TextMeshPro text
    }
    public void IncrementZombieKillCount()
    {
        zombieKillCount++;
        UpdateZombieKillUI();
    }

    // void UpdateScoreUI()
    // {
    //     scoreText.text = "Score: " + score; // Update TextMeshPro text
    // }

    void EndGame()
    {
        isGameOver = true;
        // gameOverPanel.SetActive(true);

    }

    void UpdateZombieKillUI()
    {
        zombieKillText.text = zombieKillCount + " Kills"; // Update the zombie kill counter
    }

    public void NotifyGameOver()
    {
        EndGame();
    }
}
