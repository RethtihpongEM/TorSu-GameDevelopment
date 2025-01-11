using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement; // Import TextMeshPro namespace

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance
    public float timeRemaining = 180f; // Default 3 minutes
    public TextMeshProUGUI timerText; // Reference to TextMeshProUGUI
    public TextMeshProUGUI zombieKillText;
    public TextMeshProUGUI ammoCounterText;
    public Slider healthSlider;
    public TextMeshProUGUI scoreText; // Updated for score display as well



    private int zombieKillCount = 0; // Tracks zombie kills
    private int score = 0;
    private int coinCollected = 0;
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

    public void EndGameWhenPlayerDie()
    {
        EndGame(isWinning: false);
    }

    void Start()
    {
        zombieKillCount = 0;
        score = 0;
        coinCollected = 0;
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

    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        ammoCounterText.text = currentAmmo + "/" + maxAmmo; // Update the zombie kill counter
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
            EndGame(isWinning: true);
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
        IncrementScore();
        UpdateZombieKillUI();
    }

    public void IncrementScore()
    {
        score += 100;
        // UpdateZombieKillUI();
    }

    public void IncrementCoinCollected()
    {
        coinCollected++;
        // UpdateZombieKillUI();
    }



    public void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score; // Update TextMeshPro text
    }

    // void EndGame(bool isWinning)
    // {
    //     isGameOver = true;

    //     // Stop all game activity
    //     Time.timeScale = 0; // Freeze the game

    //     // Display win or lose message
    //     if (isWinning)
    //     {
    //         Debug.Log("Player Wins!");
    //         Debug.Log("Kills: " + zombieKillCount);
    //         Debug.Log("Score: " + score);
    //         Debug.Log("Time Remaining: " + timeRemaining);
    //         Debug.Log("Coin Collected: " + coinCollected);
    //     }
    //     else
    //     {
    //         Debug.Log("Player Loses!");
    //         Debug.Log("Kills: " + zombieKillCount);
    //         Debug.Log("Score: " + score);
    //         Debug.Log("Time Remaining: " + timeRemaining);
    //         Debug.Log("Coin Collected: " + coinCollected);
    //     }

    //     // Show a game-over UI panel (optional)
    //     // if (gameOverPanel != null)
    //     // {
    //     //     gameOverPanel.SetActive(true);
    //     // }
    // }

    void EndGame(bool isWinning)
    {
        isGameOver = true;

        // Start a coroutine to handle the delay before freezing the game
        StartCoroutine(EndGameCoroutine(isWinning));
    }

    private IEnumerator EndGameCoroutine(bool isWinning)
    {
        // Display win or lose message immediately
        if (isWinning)
        {
            Debug.Log("Player Wins!");
            Debug.Log("Kills: " + zombieKillCount);
            Debug.Log("Score: " + score);
            Debug.Log("Time Remaining: " + timeRemaining);
            Debug.Log("Coin Collected: " + coinCollected);
        }
        else
        {
            Debug.Log("Player Loses!");
            Debug.Log("Kills: " + zombieKillCount);
            Debug.Log("Score: " + score);
            Debug.Log("Time Remaining: " + timeRemaining);
            Debug.Log("Coin Collected: " + coinCollected);
        }

        // Wait for 2 seconds
        yield return new WaitForSecondsRealtime(1f);

        // Freeze the game
        Time.timeScale = 0;

        // Show a game-over UI panel (optional)
        // if (gameOverPanel != null)
        // {
        //     gameOverPanel.SetActive(true);
        // }
    }

    void UpdateZombieKillUI()
    {
        zombieKillText.text = zombieKillCount + " Kills"; // Update the zombie kill counter
    }

    public void NotifyGameOver()
    {
        EndGame(isWinning: true);
    }
}
