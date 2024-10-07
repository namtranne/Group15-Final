using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public GameObject[] monsterPrefabs;
    public Transform spawnPoint;
    public TMP_Text label;
    public GameObject tileManager; // GameObject that holds TileManager
    public GameObject mainCamera;  // GameObject that holds FollowPlayer
    public GameObject scoreObject; // GameObject that holds the Score script
    public GameObject waterLayer;

    private int selectedPlayerIndex = -1;
    private int selectedBossIndex = -1;

    private string playerSelectedPath;
    private string bossSelectedPath;

    void Start()
    {
        playerSelectedPath = Application.dataPath + "/player-selected.txt";
        bossSelectedPath = Application.dataPath + "/boss-selected.txt";

        LoadSelectedData();

        int selectedCharacter = selectedPlayerIndex % characterPrefabs.Length;
        GameObject prefab = characterPrefabs[selectedCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        GameObject monster = Instantiate(monsterPrefabs[selectedBossIndex], spawnPoint.position - new Vector3(0,0,30), Quaternion.identity);

        // Get the components
        TileManager tileManagerComponent = tileManager.GetComponent<TileManager>();
        FollowPlayer followPlayer = mainCamera.GetComponent<FollowPlayer>();
        Score score = scoreObject.GetComponent<Score>();
        WaterFollow waterFollow = waterLayer.GetComponent<WaterFollow>();

        // Set the playerTransform and player references
        tileManagerComponent.playerTransform = clone.transform;
        followPlayer.player = clone.transform;
        score.player = clone.transform; // Assign player to Score script
        waterFollow.player = clone.transform;

        // Set the label text to the character prefab's name
        label.text = prefab.name;
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
}
