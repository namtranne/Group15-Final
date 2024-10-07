using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class CharacterShopManager : MonoBehaviour
{
    public Button[] playerButtons;
    public Button[] bossButtons;
    public Button buyButton;
    private Button selectedButton;

    public float selectedAlpha = 0.2f;
    public float defaultAlpha = 1f;

    private List<int> playersBought = new List<int>();
    private List<int> bossesBought = new List<int>();
    private int selectedPlayerIndex = -1;
    private int selectedBossIndex = -1;
    private int clickedPlayerIndex = -1;
    private int clickedBossIndex = -1;

    private string playersBoughtPath;
    private string bossesBoughtPath;
    private string playerSelectedPath;
    private string bossSelectedPath;
    private string currentAmountPath;

    public TextMeshPro textTooltip;
    public TextMeshPro textCurrentAmount;
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    private int currentAmount;

    private void Start()
    {
        playersBoughtPath = Application.dataPath + "/players-bought.txt";
        bossesBoughtPath = Application.dataPath + "/bosses-bought.txt";
        playerSelectedPath = Application.dataPath + "/player-selected.txt";
        bossSelectedPath = Application.dataPath + "/boss-selected.txt";
        currentAmountPath = Application.dataPath + "/current-amount.txt";

        LoadBoughtData();
        LoadSelectedData();
        LoadCurrentAmount();

        for (int i = 0; i < playerButtons.Length; i++)
        {
            int index = i;
            playerButtons[i].onClick.AddListener(() => OnCharacterSelected(playerButtons[index], index, true));
        }

        for (int i = 0; i < bossButtons.Length; i++)
        {
            int index = i;
            bossButtons[i].onClick.AddListener(() => OnCharacterSelected(bossButtons[index], index, false));
        }

        buyButton.onClick.AddListener(OnBuyButtonClicked);
        UpdateSelectedCharacters();
    }

    private void UpdateSelectedCharacters()
    {
        // Reset overlays and transparency
        foreach (var button in playerButtons)
        {
            Transform overlay = button.transform.Find("SelectedOverlay");
            if (overlay != null) overlay.gameObject.SetActive(false);

            Image image = button.transform.Find("Thumbnail").GetComponent<Image>();
            SetImageAlpha(image, defaultAlpha);
        }

        foreach (var button in bossButtons)
        {
            Transform overlay = button.transform.Find("SelectedOverlay");
            if (overlay != null) overlay.gameObject.SetActive(false);

            Image image = button.transform.Find("Thumbnail").GetComponent<Image>();
            SetImageAlpha(image, defaultAlpha);
        }

        // Show overlay for the selected player
        if (selectedPlayerIndex >= 0 && selectedPlayerIndex < playerButtons.Length)
        {
            Transform playerOverlay = playerButtons[selectedPlayerIndex].transform.Find("SelectedOverlay");
            if (playerOverlay != null) playerOverlay.gameObject.SetActive(true);
        }

        // Show overlay for the selected boss
        if (selectedBossIndex >= 0 && selectedBossIndex < bossButtons.Length)
        {
            Transform bossOverlay = bossButtons[selectedBossIndex].transform.Find("SelectedOverlay");
            if (bossOverlay != null) bossOverlay.gameObject.SetActive(true);
        }

        // Dim image for the clicked character, only one at a time
        if (clickedPlayerIndex >= 0 && !playersBought.Contains(clickedPlayerIndex))
        {
            Image image = playerButtons[clickedPlayerIndex].transform.Find("Thumbnail").GetComponent<Image>();
            SetImageAlpha(image, selectedAlpha);
        }
        else if (clickedBossIndex >= 0 && !bossesBought.Contains(clickedBossIndex))
        {
            Image image = bossButtons[clickedBossIndex].transform.Find("Thumbnail").GetComponent<Image>();
            SetImageAlpha(image, selectedAlpha);
        }
    }

    private void OnCharacterSelected(Button clickedButton, int index, bool isPlayer)
    {
        if (isPlayer)
        {
            clickedPlayerIndex = index;
            clickedBossIndex = -1; // Clear clicked boss when a player is selected

            // Update tooltip based on purchase status
            if (playersBought.Contains(index))
            {
                textTooltip.text = "BOUGHT";
                selectedPlayerIndex = index;
            }
            else if (index == 2)
            {

                textTooltip.text = "COMING SOON";
            }
            else
            {
                textTooltip.text = "PRICE: 3000";
            }
        }
        else
        {
            clickedBossIndex = index;
            clickedPlayerIndex = -1; // Clear clicked player when a boss is selected

            // Update tooltip based on purchase status
            if (bossesBought.Contains(index))
            {
                textTooltip.text = "BOUGHT";
                selectedBossIndex = index;
            }
            else
            {
                textTooltip.text = "PRICE: 3000";
            }
        }

        UpdateSelectedCharacters();
        SaveSelectedData();
    }

    public void OnBuyButtonClicked()
    {
        int price = 3000; // Assume price is 3000 for both players and bosses

        if (clickedPlayerIndex >= 0 && !playersBought.Contains(clickedPlayerIndex))
        {
            if (currentAmount >= price)
            {
                currentAmount -= price;
                playersBought.Add(clickedPlayerIndex);
                selectedPlayerIndex = clickedPlayerIndex;
                SaveBoughtData();
                SaveSelectedData();
                SaveCurrentAmount();
                textCurrentAmount.text = "Your amount: " + currentAmount;
                Debug.Log("Mua Player thành công!");
            }
            else
            {
                ShowMessage("Not enough money to buy this player!");
            }
        }
        else if (clickedBossIndex >= 0 && !bossesBought.Contains(clickedBossIndex))
        {
            if (currentAmount >= price)
            {
                currentAmount -= price;
                bossesBought.Add(clickedBossIndex);
                selectedBossIndex = clickedBossIndex;
                SaveBoughtData();
                SaveSelectedData();
                SaveCurrentAmount();
                textCurrentAmount.text = "Your amount: " + currentAmount;
                Debug.Log("Mua Boss thành công!");
            }
            else
            {
                ShowMessage("Not enough money to buy this boss!");
            }
        }
        else
        {
            ShowMessage("Character is already purchased!");
        }

        UpdateSelectedCharacters();
    }

    private void LoadBoughtData()
    {
        if (File.Exists(playersBoughtPath))
        {
            string[] lines = File.ReadAllLines(playersBoughtPath);
            foreach (string line in lines)
            {
                if (int.TryParse(line, out int index))
                {
                    playersBought.Add(index);
                }
            }
        }

        if (File.Exists(bossesBoughtPath))
        {
            string[] lines = File.ReadAllLines(bossesBoughtPath);
            foreach (string line in lines)
            {
                if (int.TryParse(line, out int index))
                {
                    bossesBought.Add(index);
                }
            }
        }
    }

    private void SaveBoughtData()
    {
        File.WriteAllLines(playersBoughtPath, playersBought.ConvertAll(index => index.ToString()).ToArray());
        File.WriteAllLines(bossesBoughtPath, bossesBought.ConvertAll(index => index.ToString()).ToArray());
    }

    private void LoadSelectedData()
    {
        if (File.Exists(playerSelectedPath))
        {
            string content = File.ReadAllText(playerSelectedPath);
            if (int.TryParse(content, out int index))
            {
                selectedPlayerIndex = index;
            }
        }

        if (File.Exists(bossSelectedPath))
        {
            string content = File.ReadAllText(bossSelectedPath);
            if (int.TryParse(content, out int index))
            {
                selectedBossIndex = index;
            }
        }
    }

    private void SaveSelectedData()
    {
        File.WriteAllText(playerSelectedPath, selectedPlayerIndex.ToString());
        File.WriteAllText(bossSelectedPath, selectedBossIndex.ToString());
    }

    private void SetImageAlpha(Image image, float alphaValue)
    {
        Color color = image.color;
        color.a = alphaValue;
        image.color = color;
    }

    private void LoadCurrentAmount()
    {
        if (File.Exists(currentAmountPath))
        {
            string content = File.ReadAllText(currentAmountPath);
            if (int.TryParse(content, out int amount))
            {
                currentAmount = amount;
            }
        }
        else
        {
            currentAmount = 0; // Default value if file does not exist
        }

        textCurrentAmount.text = $"Your amount: {currentAmount}";
    }

    private void SaveCurrentAmount()
    {
        File.WriteAllText(currentAmountPath, currentAmount.ToString());
        textCurrentAmount.text = $"Your amount: {currentAmount}";
    }

    private void ShowMessage(string message)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
        Invoke(nameof(HideMessage), 2f); // Hide the message after 2 seconds
    }

    private void HideMessage()
    {
        messagePanel.SetActive(false);
    }

    public void UnHoverButton()
    {
        textTooltip.text = "";
    }
}
