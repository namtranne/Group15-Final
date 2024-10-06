using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    private Dictionary<string, float> powerUps = new Dictionary<string, float>();
    private float powerUpDuration = 5f; // Duration for all power-ups
    public static PowerUpManager instance;

    // UI elements: Texts for remaining time and Images for the power-up icons
    public Text[] powerCount;    // Text array for remaining time display
    public Image[] powerImages;  // Image array for power-up icons
    public Sprite[] powerSprites; // Power-up sprites

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Initially disable all images and texts
        ResetPowerUpUI();
    }

    void Update()
    {
        UpdatePowerUps();
        UpdatePowerUpUI();
    }

    // Method to collect a power-up
    public void CollectPowerUp(string powerUpName)
    {
        if (!powerUps.ContainsKey(powerUpName))
        {
            powerUps[powerUpName] = powerUpDuration;
            Debug.Log($"{powerUpName} collected! Duration: {powerUpDuration} seconds.");
        }
        else
        {
            if(powerUps[powerUpName] > 0) {
                powerUps[powerUpName] += powerUpDuration;
            }
            powerUps[powerUpName] = powerUpDuration;
            Debug.Log($"{powerUpName} collected again! Duration reset to {powerUpDuration} seconds.");
        }
    }

    // Method to update power-ups duration
    private void UpdatePowerUps()
    {
        List<string> powerUpKeys = new List<string>(powerUps.Keys);

        foreach (var powerUp in powerUpKeys)
        {
            if (powerUps[powerUp] >= 0)
                powerUps[powerUp] -= Time.deltaTime;

            if (powerUps[powerUp] <= 0)
            {
                Debug.Log($"{powerUp} has expired. Duration: {powerUps[powerUp]} seconds.");
            }
        }
    }

    // Reset the UI: Disable all power-up images and text
    private void ResetPowerUpUI()
    {
        for (int i = 0; i < powerImages.Length; i++)
        {
            powerImages[i].gameObject.SetActive(false); // Disable images
            powerCount[i].gameObject.SetActive(false);  // Disable text
        }
    }

    // Method to update the UI with current active power-ups and their remaining time
    private void UpdatePowerUpUI()
    {
        // Sort the power-ups by remaining time in descending order
        var sortedPowerUps = new List<KeyValuePair<string, float>>(powerUps);
        sortedPowerUps.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

        ResetPowerUpUI(); // Reset before updating the UI

        int index = 0; // Track how many power-ups are being displayed
        foreach (var powerUp in sortedPowerUps)
        {
            if (powerUp.Value > 0 && index < powerImages.Length)
            {
                // Enable the corresponding image and text
                powerImages[index].gameObject.SetActive(true);
                powerCount[index].gameObject.SetActive(true);

                // Set the power-up sprite
                powerImages[index].sprite = GetPowerUpSprite(powerUp.Key);

                // Set the power-up remaining time in the text
                powerCount[index].text = $"{powerUp.Value:F1}s";

                index++; // Move to the next UI element
            }
        }
    }

    // Method to get the corresponding sprite for each power-up
    private Sprite GetPowerUpSprite(string powerUpName)
    {
        switch (powerUpName)
        {
            case "Shield":
                return powerSprites[0];
            case "Intangible":
                return powerSprites[1];
            case "Jump":
                return powerSprites[2];
            default:
                return null;
        }
    }

    // Method to check if a power-up is active
    public bool IsPowerUpActive(string powerUpName)
    {
        return powerUps.ContainsKey(powerUpName) && powerUps[powerUpName] > 0;
    }
}
