using System.IO;
using UnityEngine;
using TMPro;

public class LoadCurrentAmount : MonoBehaviour
{
    public TextMeshPro textDisplay;
    private string filePath;

    void Start()
    {
        // filePath = Application.dataPath + "/current-amount.txt";
        // LoadText();
        // Debug.Log("filePath: " + filePath);
    }

    public void SaveText(string content)
    {
        File.WriteAllText(filePath, content);
        Debug.Log("Text saved to " + filePath);
    }

    public void LoadText()
    {
        if (File.Exists(filePath))
        {
            string content = File.ReadAllText(filePath);
            textDisplay.text = "Current amount: " + content + "$";
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
        }
    }
}
