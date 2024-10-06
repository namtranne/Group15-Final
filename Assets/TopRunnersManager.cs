using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopRunnersManager : MonoBehaviour
{
    public TextMeshPro textDisplay;
    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/top-runners.txt";
        LoadText();
        Debug.Log("filePath: " + filePath);
    }

    public void AddRunner(string name, int score)
    {
        List<(string name, int score)> runners = LoadRunnersFromFile();

        runners.Add((name, score));

        var topRunners = runners.OrderByDescending(r => r.score).Take(6).ToList();
        SaveTopRunnersToFile(topRunners);
    }

    private List<(string name, int score)> LoadRunnersFromFile()
    {
        List<(string name, int score)> runners = new List<(string name, int score)>();

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);

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

        File.WriteAllLines(filePath, lines);

        textDisplay.text = string.Join("\n", lines);
        Debug.Log("Top runners saved.");
    }

    public void LoadText()
    {
        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            textDisplay.text = content;
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }
}
