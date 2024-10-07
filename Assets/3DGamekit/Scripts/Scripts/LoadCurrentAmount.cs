using System.IO;
using UnityEngine;
using TMPro;
using System; 

public class LoadCurrentAmount : MonoBehaviour
{
    public TextMeshPro textDisplay;
    private string filePath;

    void Start()
    {
        filePath = Application.dataPath + "/current-amount.txt";
        // LoadText();
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
            File.WriteAllText(filePath, "0");
            string content = File.ReadAllText(filePath);
            textDisplay.text = "Current amount: " + content + "$";
        }
    }


    public int ModifyAmount(int amount) {
        if (File.Exists(filePath) == false ) {
            File.WriteAllText(filePath, "0");
        }
        
        string content = File.ReadAllText(filePath);
        int currentAmount =  Int32.Parse(content);
        if(currentAmount < 0) currentAmount = 0;
        currentAmount = currentAmount + amount;
        SaveText(currentAmount.ToString());
        return currentAmount;
    }

    public int LoadAmount() {
        if (File.Exists(filePath) == false ) {
            File.WriteAllText(filePath, "0");
        }
        return 0;
    }
}
