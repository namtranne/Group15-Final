using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Make sure you are using this for UI elements like Text

public class GameManager : MonoBehaviour
{
    bool isEnded = false;
    public float restartDelay = 1f;
    private int currentCrystals = 0;
    public GameObject crystalText;  // This should be a UI Text element
    public GameObject scoreText;
    public GameObject completeLevelUI;
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }

    public void EndGame()
    {
        if (isEnded == false)
        {
            isEnded = true;
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        Text crystalTextComponent = crystalText.GetComponent<Text>();
        int crystal_text = int.Parse(crystalTextComponent.text);

       
        Text scoreTextComponent = scoreText.GetComponent<Text>();
        int score_text = int.Parse(scoreTextComponent.text);

       
        PlayerPrefs.SetInt("crystalTextComponent", crystal_text);
        PlayerPrefs.SetInt("scoreTextComponent", score_text);

       
        SceneManager.LoadScene(2);

        
    }

    // Method to add crystals and update UI
    public void AddCrystals(int value)
    {
        currentCrystals += value;
        
        // Update the UI text to reflect the crystal count
        Text crystalTextComponent = crystalText.GetComponent<Text>();
        crystalTextComponent.text = currentCrystals.ToString();  // Set the text value
    }
}
