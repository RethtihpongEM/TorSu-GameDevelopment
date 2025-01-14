using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    private string filePath;
    public int kill;   // Variable to store the last line's kills
    public int score;  // Variable to store the last line's score
    public int count;  // Variable to store the last line's coins
    public int[] kills;   // Array to store all kills
    public int[] scores;  // Array to store all scores
    public int[] coins;   // Array to store all coins
    public int totalKills;   // Sum of all kills
    public int totalScores;  // Sum of all scores
    public int totalCoins;   // Sum of all coins
    public TextMeshProUGUI previousKillText;
    public TextMeshProUGUI previousScoreText;
    public TextMeshProUGUI previousCoinText;
    public TextMeshProUGUI totalKillText;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI totalCoinText;

    void Start()
    {
        // Define the file path within the Data folder
        string dataFolderPath = Path.Combine(Application.dataPath, "Data");
        filePath = Path.Combine(dataFolderPath, "Leaderboard.txt");

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Read all lines
            string[] lines = File.ReadAllLines(filePath);

            // Process the last line
            if (lines.Length > 0)
            {
                string lastLine = lines[lines.Length - 1];
                ParseLeaderboardData(lastLine);
            }

            // Process all lines to calculate totals
            ProcessAllLeaderboardData(lines);
        }
        else
        {
            Debug.LogError("Leaderboard.txt does not exist.");
        }
    }

    // Method to process the last line of the leaderboard file
    private void ParseLeaderboardData(string data)
    {
        try
        {
            string[] values = data.Split(',');
            if (values.Length == 3)
            {
                kill = int.Parse(values[0]);
                score = int.Parse(values[1]);
                count = int.Parse(values[2]);

                Debug.Log($"Last Leaderboard Data - Kills: {kill}, Score: {score}, Coins: {count}");

                // Update the UI text
                if (previousKillText != null) previousKillText.text = kill.ToString();
                if (previousScoreText != null) previousScoreText.text = score.ToString();
                if (previousCoinText != null) previousCoinText.text = count.ToString();
            }
            else
            {
                Debug.LogError("Data format in the last line of Leaderboard.txt is incorrect.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing last line data: {e.Message}");
        }
    }

    // Method to process all lines and calculate totals
    private void ProcessAllLeaderboardData(string[] lines)
    {
        // Initialize arrays
        int lineCount = lines.Length;
        kills = new int[lineCount];
        scores = new int[lineCount];
        coins = new int[lineCount];

        // Parse each line
        for (int i = 0; i < lineCount; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length == 3)
            {
                kills[i] = int.Parse(values[0]);
                scores[i] = int.Parse(values[1]);
                coins[i] = int.Parse(values[2]);
            }
            else
            {
                Debug.LogError($"Invalid data format on line {i + 1}: {lines[i]}");
            }
        }

        // Calculate totals
        totalKills = SumArray(kills);
        totalScores = SumArray(scores);
        totalCoins = SumArray(coins);

        Debug.Log($"Total Kills: {totalKills}, Total Scores: {totalScores}, Total Coins: {totalCoins}");

        // Update the total UI text
        if (totalKillText != null) totalKillText.text = totalKills.ToString();
        if (totalScoreText != null) totalScoreText.text = totalScores.ToString();
        if (totalCoinText != null) totalCoinText.text = totalCoins.ToString();
    }

    // Method to calculate the sum of an integer array
    private int SumArray(int[] array)
    {
        int sum = 0;
        foreach (int value in array)
        {
            sum += value;
        }
        return sum;
    }
}
