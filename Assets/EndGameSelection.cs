using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System; // Added to import the System namespace

public class EndGameSelection : MonoBehaviour
{
    public GameObject crystalText;
    public GameObject scoreText;

    private string scorefilePath;
    private string gemfilePath;

    public static EndGameSelection instance;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        int currentCrystal = PlayerPrefs.GetInt("crystalTextComponent");
        int currentScore = PlayerPrefs.GetInt("scoreTextComponent");

        Text crystalTextComponent = crystalText.GetComponent<Text>();
        crystalTextComponent.text = currentCrystal.ToString();

        Text scoreTextComponent = scoreText.GetComponent<Text>();
        scoreTextComponent.text = currentScore.ToString();

        scorefilePath = Application.dataPath + "/top-runners.txt";
        gemfilePath = Application.dataPath + "/current_amount.txt";

        // ScoreLoadText();
        // LoadText();
    }

    public void QuitButton()
    {
        int currentCrystal = PlayerPrefs.GetInt("crystalTextComponent");
        int currentScore = PlayerPrefs.GetInt("scoreTextComponent");
        ModifyAmount(currentCrystal);
        ScoreAddRunner("Player", currentScore);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void StartGame()
    {
        int currentCrystal = PlayerPrefs.GetInt("crystalTextComponent");
        int currentScore = PlayerPrefs.GetInt("scoreTextComponent");
        ModifyAmount(currentCrystal);
        ScoreAddRunner("Player", currentScore);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void ScoreAddRunner(string name, int score)
    {
        List<(string name, int score)> runners = ScoreLoadRunnersFromFile();

        runners.Add((name, score));

        var topRunners = runners.OrderByDescending(r => r.score).Take(6).ToList();
        SaveTopRunnersToFile(topRunners);
    }

    private List<(string name, int score)> ScoreLoadRunnersFromFile()
    {
        List<(string name, int score)> runners = new List<(string name, int score)>();

        if (File.Exists(scorefilePath))
        {
            string[] lines = File.ReadAllLines(scorefilePath);

            foreach (var line in lines)
            {
                string[] parts = line.Split(new[] { " - " }, StringSplitOptions.None);

                if (parts.Length == 2)
                {
                    string runnerName = parts[0].Substring(parts[0].IndexOf(' ') + 1).Trim();
                    if (int.TryParse(parts[1].Trim(), out int runnerScore))
                    {
                        runners.Add((runnerName, runnerScore));
                    }
                }
            }
        }

        return runners;
    }

    private void SaveTopRunnersToFile(List<(string name, int score)> topRunners)
    {
        List<string> lines = new List<string>();

        for (int i = 0; i < topRunners.Count; i++)
        {
            var runner = topRunners[i];
            lines.Add($"{i + 1}. {runner.name} - {runner.score}");
        }

        File.WriteAllLines(scorefilePath, lines);

        Text scoreTextComponent = scoreText.GetComponent<Text>();
        Debug.Log("Top runners saved.");
    }

    public void ScoreLoadText()
    {
        if (File.Exists(scorefilePath))
        {
            string content = File.ReadAllText(scorefilePath);
            Text scoreTextComponent = scoreText.GetComponent<Text>();
            scoreTextComponent.text = content;
        }
        else
        {
            Debug.LogWarning("File not found: " + scorefilePath);
        }
    }

    public void SaveText(string content)
    {
        File.WriteAllText(gemfilePath, content);
        Debug.Log("Text saved to " + gemfilePath);
    }

    public void LoadText()
    {
        if (File.Exists(gemfilePath))
        {
            string content = File.ReadAllText(gemfilePath);
            Text crystalTextComponent = crystalText.GetComponent<Text>();
            crystalTextComponent.text = content;
        }
        else
        {
            File.WriteAllText(gemfilePath, "0");
            string content = File.ReadAllText(gemfilePath);
            Text crystalTextComponent = crystalText.GetComponent<Text>();
            crystalTextComponent.text = content;
        }
    }

    public int ModifyAmount(int amount)
    {
        if (File.Exists(gemfilePath) == false)
        {
            File.WriteAllText(gemfilePath, "0");
        }
        
        string content = File.ReadAllText(gemfilePath);
        int currentAmount = int.Parse(content);
        if (currentAmount < 0) currentAmount = 0;
        currentAmount = currentAmount + amount;
        SaveText(currentAmount.ToString());
        return currentAmount;
    }

    public int LoadAmount()
    {
        if (File.Exists(gemfilePath) == false)
        {
            File.WriteAllText(gemfilePath, "0");
        }
        return 0;
    }
}