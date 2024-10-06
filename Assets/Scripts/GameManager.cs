using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Make sure you are using this for UI elements like Text

public class GameManager : MonoBehaviour
{
    bool isEnded = false;
    public float restartDelay = 1f;
    private int currentCrystals = 0;
    public GameObject crystalText;  // This should be a UI Text element
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
