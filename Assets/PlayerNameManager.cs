using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PlayerNameManager : MonoBehaviour
{
    public TMP_InputField inputField;
    private string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.dataPath, "player-name.txt");
        Debug.Log(filePath);
        LoadInput();
    }

    public void OnEndEdit()
    {
        string value = inputField.text;
        SaveInput(value);
    }

    private void SaveInput(string value)
    {
        File.WriteAllText(filePath, value);
        Debug.Log("Saved player name: " + value);
    }

    private void LoadInput()
    {
        if (File.Exists(filePath))
        {
            string loadedText = File.ReadAllText(filePath);
            inputField.text = loadedText;
            Debug.Log("Loaded player name: " + loadedText);
        }
        else
        {
            Debug.Log("No previous input found.");
        }
    }
}
