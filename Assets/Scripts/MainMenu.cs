using System.IO; // For file handling
using TMPro; // For TextMeshPro
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private string filePath;
    public GameObject leaderboardPanel;

    void Start()
    {
        // Define the file path within the Data folder
        string dataFolderPath = Path.Combine(Application.dataPath, "Data");
        filePath = Path.Combine(dataFolderPath, "Coin.txt");

        // Ensure the folder and file exist
        if (!Directory.Exists(dataFolderPath))
        {
            Directory.CreateDirectory(dataFolderPath); // Create the folder if it doesn't exist
        }

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "0"); // Create the file with an initial value of 0
        }

        // Read coin value from the file and display it
        UpdateCoinDisplay();
    }

    // Reads the coin value from the file and updates the coinText UI
    void UpdateCoinDisplay()
    {
        try
        {
            string coinValue = File.ReadAllText(filePath); // Read the content of the file

            // Validate and set the coin value
            if (int.TryParse(coinValue, out int coins))
            {
                coinText.text = $"{coins}";
            }
            else
            {
                Debug.LogWarning("Coin.txt contains invalid data. Resetting to 0.");
                File.WriteAllText(filePath, "0"); // Reset file content if invalid
                coinText.text = "0";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error reading coin data: {e.Message}");
            coinText.text = "0"; // Fallback if reading fails
        }
    }

    public void leaderboardPanelUI() {
        leaderboardPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void closeLeaderboardPanelUI() {
        leaderboardPanel.SetActive(false);
        Time.timeScale = 0f;
    }
}
