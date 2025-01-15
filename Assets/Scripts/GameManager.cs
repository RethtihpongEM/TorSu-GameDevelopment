using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameObject timeUpPanel;  // Assign your WinPanel in the Inspector
    public GameObject losePanel; // Assign your LosePanel in the Inspector
    public static GameManager Instance; // Singleton instance
    public float timeRemaining = 180f; // Default 3 minutes
    public TextMeshProUGUI timerText; // Reference to TextMeshProUGUI
    public TextMeshProUGUI zombieKillText;
    public TextMeshProUGUI ammoCounterText;
    public TextMeshProUGUI coinCollectedText; // UI for coin collected during gameplay
    public Slider healthSlider;
    public TextMeshProUGUI resultScoreText, resultCoinText, restultKillText;
    public TextMeshProUGUI timeUpScoreText, timeUpKillText, timeUpCoinText;
    private int zombieKillCount = 0, score = 0, coinCollected = 0;
    private bool isGameOver = false;
    private string filePath;

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

        // Define the file path within the Data folder
        string dataFolderPath = Path.Combine(Application.dataPath, "Data");
        if (!Directory.Exists(dataFolderPath))
        {
            Directory.CreateDirectory(dataFolderPath); // Create Data folder if it doesn't exist
            Debug.Log("Data folder created at: " + dataFolderPath);
        }

        filePath = Path.Combine(dataFolderPath, "Coin.txt");
        Debug.Log("File path initialized: " + filePath);
    }

    void Start()
    {
        // Initialize values and update UI
        zombieKillCount = 0;
        score = 0;
        coinCollected = 0;

        UpdateZombieKillUI();
        UpdateScoreUI();
        UpdateTimerUI();
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
            TriggerTimeUp(); // Time ran out, player loses
        }
    }

    // Updates the health slider UI
    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // Updates the ammo counter UI
    public void UpdateAmmoUI(int currentAmmo, int maxAmmo)
    {
        if (ammoCounterText != null)
        {
            ammoCounterText.text = $"{currentAmmo}/{maxAmmo}";
        }
    }

    // Updates the timer UI
    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Increment zombie kill count
    public void IncrementZombieKillCount()
    {
        zombieKillCount++;
        IncrementScore();
        IncrementScore();
        UpdateZombieKillUI();
        UpdateKillUI();
    }

    // Increment score
    public void IncrementScore()
    {
        score += 100; // Example: +100 per kill
        UpdateScoreUI();
    }

    // Increment coins collected
    public void IncrementCoinCollected()
    {
        coinCollected++; // Logic for coins
        UpdateCoinUI();
        UpdateCoinCollectedUI();
    }

    // Updates the zombie kill counter UI
    void UpdateZombieKillUI()
    {
        if (zombieKillText != null)
        {
            zombieKillText.text = $"{zombieKillCount} Kills";
        }
    }
    void UpdateCoinCollectedUI()
    {
        if (coinCollectedText != null)
        {
            coinCollectedText.text = $"{coinCollected}";
        }
    }

    // Saves coinCollected to a .txt file
    public void SaveCoinData()
    {
        int totalCoins = 0;

        try
        {
            // Step 1: Read existing total from the file
            if (File.Exists(filePath))
            {
                string[] coinData = File.ReadAllLines(filePath);
                foreach (string line in coinData)
                {
                    if (int.TryParse(line, out int coinValue))
                    {
                        totalCoins += coinValue;
                    }
                }
                Debug.Log($"Total coins from file before adding new: {totalCoins}");
            }

            // Step 2: Add the current session's collected coins
            totalCoins += coinCollected;
            Debug.Log($"New total coins after adding session coins: {totalCoins}");

            // Step 3: Save the updated total back to the file
            File.WriteAllText(filePath, totalCoins.ToString() + "\n");

            // Step 4: Update the UI
            if (coinCollectedText != null)
            {
                coinCollectedText.text = $"{totalCoins}";
            }

            Debug.Log("Total coins saved successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving coin data: {e.Message}");
        }
    }

    // Saves leaderboard data to Leaderboard.txt
    void SaveLeaderboardData()
    {
        try
        {
            // Define the Leaderboard file path
            string leaderboardFilePath = Path.Combine(Application.dataPath, "Data", "Leaderboard.txt");

            // Ensure the Data folder exists
            string dataFolderPath = Path.GetDirectoryName(leaderboardFilePath);
            if (!Directory.Exists(dataFolderPath))
            {
                Directory.CreateDirectory(dataFolderPath);
            }

            // Format the data to append
            string leaderboardEntry = $"{zombieKillCount},{score},{coinCollected}\n";

            // Append the leaderboard entry to the file
            File.AppendAllText(leaderboardFilePath, leaderboardEntry);

            Debug.Log("Leaderboard entry saved successfully: " + leaderboardEntry);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving leaderboard data: {e.Message}");
        }
    }

    void UpdateKillUI()
    {
        if (restultKillText != null)
        {
            restultKillText.text = $"Kills:\n{zombieKillCount}";
        }
        if (timeUpKillText != null)
        {
            timeUpKillText.text = $"Kills:\n{zombieKillCount}";
        }
    }

    // Updates the score UI
    void UpdateScoreUI()
    {
        if (resultScoreText != null)
        {
            resultScoreText.text = $"Score:\n{score}";
        }
        if (timeUpScoreText != null)
        {
            timeUpScoreText.text = $"Score:\n{score}";
        }
    }
    void UpdateCoinUI()
    {
        if (resultCoinText != null)
        {
            resultCoinText.text = $"Coins:\n{coinCollected}";
        }
        if (timeUpCoinText != null)
        {
            timeUpCoinText.text = $"Coins:\n{coinCollected}";
        }
    }

    // Ends the game with win or lose condition
    void EndGame(bool isTimeUp)
    {
        if (isGameOver) return; // Prevent multiple triggers

        isGameOver = true;

        // Stop the game and show the corresponding panel
        Time.timeScale = 0;
        if (isTimeUp)
        {
            // UpdateTimeUpPanelUI();
            timeUpPanel.SetActive(true); // Show the win panel
        }
        else
        {
            losePanel.SetActive(true); // Show the lose panel
        }

        // Save coins are collected
        SaveCoinData();

        // Save leaderboard data
        SaveLeaderboardData();

        // Debug logs (optional for developer reference)
        Debug.Log(isTimeUp ? "Time's Up!" : "Player Loses!");
        Debug.Log($"Kills: {zombieKillCount}");
        Debug.Log($"Score: {score}");
        Debug.Log($"Time Remaining: {timeRemaining}");
        Debug.Log($"Coins Collected: {coinCollected}");
    }

    // Triggers win condition
    public void TriggerTimeUp()
    {
        // UpdateTimeUpPanelUI();
        EndGame(isTimeUp: true);
    }

    // Triggers lose condition
    public void TriggerLose()
    {
        EndGame(isTimeUp: false);
    }

}