using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private string filePath;
    public int kill, score, count;
    public int[] kills, scores, coins;
    public int totalKills, totalScores, totalCoins;

    public TextMeshProUGUI previousKillText, previousScoreText, previousCoinText;
    public TextMeshProUGUI totalKillText, totalScoreText, totalCoinText;

    void Start()
    {
        string dataFolderPath = Path.Combine(Application.dataPath, "Data");
        filePath = Path.Combine(dataFolderPath, "Leaderboard.txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length > 0)
                ParseLeaderboardData(lines[^1]); // Process last line

            ProcessAllLeaderboardData(lines); // Process all lines
        }
        else
        {
            Debug.LogWarning("Leaderboard.txt not found. Ensure the file exists.");
        }
    }

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

                UpdateUIText(previousKillText, kill);
                UpdateUIText(previousScoreText, score);
                UpdateUIText(previousCoinText, count);
            }
            else
            {
                Debug.LogError("Invalid data format in the last line.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing leaderboard data: {ex.Message}");
        }
    }

    private void ProcessAllLeaderboardData(string[] lines)
    {
        int lineCount = lines.Length;
        kills = new int[lineCount];
        scores = new int[lineCount];
        coins = new int[lineCount];

        for (int i = 0; i < lineCount; i++)
        {
            string[] values = lines[i].Split(',');
            try
            {
                if (values.Length == 3)
                {
                    kills[i] = int.Parse(values[0]);
                    scores[i] = int.Parse(values[1]);
                    coins[i] = int.Parse(values[2]);

                    totalKills += kills[i];
                    totalScores += scores[i];
                    totalCoins += coins[i];
                }
                else
                {
                    Debug.LogError($"Invalid data on line {i + 1}: {lines[i]}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error processing line {i + 1}: {ex.Message}");
            }
        }

        UpdateUIText(totalKillText, totalKills);
        UpdateUIText(totalScoreText, totalScores);
        UpdateUIText(totalCoinText, totalCoins);
    }

    private void UpdateUIText(TextMeshProUGUI textElement, int value)
    {
        if (textElement != null)
            textElement.text = value.ToString();
    }
}
