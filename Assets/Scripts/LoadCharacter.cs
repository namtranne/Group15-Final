﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public GameObject[] monsterPrefabs;
    public Transform spawnPoint;
    public TMP_Text label;
    public GameObject tileManager; // GameObject that holds TileManager
    public GameObject mainCamera;  // GameObject that holds FollowPlayer
    public GameObject scoreObject; // GameObject that holds the Score script

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        GameObject prefab = characterPrefabs[selectedCharacter];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        GameObject monster = Instantiate(monsterPrefabs[0], spawnPoint.position - new Vector3(0,0,30), Quaternion.identity);

        // Get the components
        TileManager tileManagerComponent = tileManager.GetComponent<TileManager>();
        FollowPlayer followPlayer = mainCamera.GetComponent<FollowPlayer>();
        Score score = scoreObject.GetComponent<Score>();

        // Set the playerTransform and player references
        tileManagerComponent.playerTransform = clone.transform;
        followPlayer.player = clone.transform;
        followPlayer.playerMovement = clone.GetComponent<PlayerMovement>();
        score.player = clone.transform; // Assign player to Score script

        // Set the label text to the character prefab's name
        label.text = prefab.name;
    }
}
