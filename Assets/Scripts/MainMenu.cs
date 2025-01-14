using System.IO; // For file handling
using TMPro; // For TextMeshPro
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI coinText; // Text element to display coin count
    public GameObject leaderboardPanel; // Leaderboard UI panel

    private string filePath;

    void Start()
    {
        InitializeFilePath();
        EnsureDataFileExists();
        UpdateCoinDisplay();
    }

    /// <summary>
    /// Initializes the file path for the coin data file and ensures the folder exists.
    /// </summary>
    private void InitializeFilePath()
    {
        // Use persistentDataPath for better compatibility across platforms
        string dataFolderPath = Path.Combine(Application.dataPath, "Data");
        filePath = Path.Combine(dataFolderPath, "Coin.txt");

        // Create the data folder if it doesn't exist
        if (!Directory.Exists(dataFolderPath))
        {
            Directory.CreateDirectory(dataFolderPath);
        }
    }

    /// <summary>
    /// Ensures the coin data file exists. If not, creates it with a default value of 0.
    /// </summary>
    private void EnsureDataFileExists()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("Coin.txt not found. Creating a new file with default value 0.");
            File.WriteAllText(filePath, "0");
        }
    }

    /// <summary>
    /// Reads the coin value from the file and updates the UI display.
    /// </summary>
    private void UpdateCoinDisplay()
    {
        try
        {
            string coinValue = File.ReadAllText(filePath);

            if (int.TryParse(coinValue, out int coins))
            {
                coinText.text = coins.ToString();
            }
            else
            {
                HandleInvalidCoinData();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error reading coin data: {e.Message}");
            ResetCoinData();
        }
    }

    /// <summary>
    /// Handles invalid data in the coin file by resetting it to 0.
    /// </summary>
    private void HandleInvalidCoinData()
    {
        Debug.LogWarning("Coin.txt contains invalid data. Resetting to 0.");
        ResetCoinData();
    }

    /// <summary>
    /// Resets the coin data file and updates the UI display to 0.
    /// </summary>
    private void ResetCoinData()
    {
        File.WriteAllText(filePath, "0");
        coinText.text = "0";
    }

    /// <summary>
    /// Opens the leaderboard panel and pauses the game.
    /// </summary>
    public void OpenLeaderboardPanel()
    {
        leaderboardPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    /// <summary>
    /// Closes the leaderboard panel and resumes the game.
    /// </summary>
    public void CloseLeaderboardPanel()
    {
        leaderboardPanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}