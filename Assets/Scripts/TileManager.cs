using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
// using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    //public GameObject[] tilesPrefabs;
    public GameObject[] obstacleTiles;
    private int totalTiles = 0;
    public GameObject[] bridgeObstacles;
    public GameObject[] crystalPatterns;
    public Transform[] powerUpPositions;
    public GameObject[] powerUps;
    public GameObject coinTiles;
    public GameObject roadObject;
    public GameObject[] fallingRoadObjects;
    public GameObject bridgeObject;
    public GameObject shortBridgeObject;
    public float zSpawn = 0;
    public float tileLength=26.0f;
    public int numberOfTiles=5;
    public Transform playerTransform;
    private List<GameObject> activeTile = new List<GameObject>();
    private bool isGeneratingPowerUp = false;

    // Start is called before the first frame update
    void Start()
    {

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        
        // coinObject = new Coin(coinTiles);
        for(int i=0;i<numberOfTiles;i++)
        {
            if (i == 0) GeneratePath(true);
            else GeneratePath();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z - tileLength - 20>= zSpawn-(numberOfTiles*tileLength))
        {
            GeneratePath();
            DeleteTile();
        }
    }
    private void DeleteTile()
    {
        Destroy(activeTile[0]);
        activeTile.RemoveAt(0);
    }

    private void GeneratePath(bool start=false)
    {
        int obstacleIndex = Random.Range(0, 4);
        if(totalTiles < 3) {
            obstacleIndex = 0;
        }

        if(totalTiles > 0 && totalTiles % 5 == 0) {
            isGeneratingPowerUp = true;
        }

        totalTiles++;
        switch(obstacleIndex) {
            case 0 : {
                GenerateRoad();
                break;
            }
            case 1: {
                GenerateBridge();
                break;
            }
            case 2: {
                GenerateShortBridge();
                break;
            }
            case 3: {
                GenerateFallingRoad();
                break;
            }
        }
    }


    private void GenerateRoad() {
        GameObject roadGo = Instantiate(roadObject, transform.forward * zSpawn + new Vector3(2,0,0), Quaternion.identity);
        zSpawn += tileLength;
        activeTile.Add(roadGo);
        // return;

        if(totalTiles < 3) {
            return;
        }

        int obstacleIndex = Random.Range(0, obstacleTiles.Length);

        GameObject obsGo = Instantiate(obstacleTiles[obstacleIndex], 
                                roadGo.transform.position, roadGo.transform.rotation);
        obsGo.transform.SetParent(roadGo.transform);
        obsGo.transform.localPosition = obstacleTiles[obstacleIndex].transform.localPosition;
        obsGo.transform.localRotation = obstacleTiles[obstacleIndex].transform.localRotation;

        bool isGeneratingCrystal = Random.Range(0, 2)  == 1 && !isGeneratingPowerUp;
        // if(!isGeneratingCrystal) return;

        if(isGeneratingCrystal) {
            GameObject crystalPattern = Instantiate(crystalPatterns[obstacleIndex], 
                                roadGo.transform.position, roadGo.transform.rotation);
            crystalPattern.transform.SetParent(roadGo.transform);
            crystalPattern.transform.localPosition = crystalPatterns[obstacleIndex].transform.localPosition;
            crystalPattern.transform.localRotation = crystalPatterns[obstacleIndex].transform.localRotation;
        }
        
        if(!isGeneratingPowerUp) return;
        isGeneratingPowerUp = false;

        int powerUpIndex = Random.Range(0, powerUps.Length);
        GameObject powerUpItem = Instantiate(powerUps[powerUpIndex], 
                                roadGo.transform.position, powerUps[powerUpIndex].transform.localRotation);

        powerUpItem.transform.SetParent(roadGo.transform);
        powerUpItem.transform.localPosition = powerUpPositions[obstacleIndex].transform.localPosition;
    }

    private void GenerateBridge() {
        GameObject bridge = Instantiate(bridgeObject, transform.forward * zSpawn + new Vector3(0,0,12), Quaternion.identity);

        int obstacleIndex = Random.Range(0, bridgeObstacles.Length);
        GameObject obsGo = Instantiate(bridgeObstacles[obstacleIndex], 
                                bridge.transform.position, bridge.transform.rotation);
        obsGo.transform.SetParent(bridge.transform);
        obsGo.transform.localPosition = bridgeObstacles[obstacleIndex].transform.localPosition;
        obsGo.transform.localRotation = bridgeObstacles[obstacleIndex].transform.localRotation;
        zSpawn += tileLength;
        activeTile.Add(bridge);
    }

    private void GenerateShortBridge() {
        GameObject bridge = Instantiate(shortBridgeObject, transform.forward * zSpawn + new Vector3(0,0,18), Quaternion.identity);
        activeTile.Add(bridge);
        zSpawn += tileLength;
    }

    private void GenerateFallingRoad() {
        int obstacleIndex = Random.Range(0, 2);
        GameObject roadGo = Instantiate(fallingRoadObjects[obstacleIndex], transform.forward * zSpawn + new Vector3(2,0,0), Quaternion.identity);
        zSpawn += tileLength;
        activeTile.Add(roadGo);
    }

    // private void GenerateCoin(GameObject roadObject)
    // {
    //     int choiceBeforeAfter = Random.Range(0, 2);
    //     //0: before, 1: after, 2: both
    //     int numOfCoins = Random.Range(0, 4);
    //     int[] lanes = Enumerable.Range(0, 3).OrderBy(x => Random.Range(0, 3)).Take(numOfCoins).ToArray();
        
    //     for (int i = 1; i <= numOfCoins; i++)
    //     {
    //         Lane lane = (Lane)lanes[i - 1];
    //         if (choiceBeforeAfter ==0 || choiceBeforeAfter==2)
    //         {
    //             Vector3 coinPos = coinObject.PositionBefore(lane);
    //             GameObject coinGo = Instantiate(coinTiles, new Vector3(coinPos.x, coinPos.y, coinPos.z+ transform.forward.z * zSpawn), Quaternion.AngleAxis(90, Vector3.right));
    //             coinGo.transform.parent = roadObject.transform;
    //         }
    //         if(choiceBeforeAfter == 1 || choiceBeforeAfter == 2)
    //         {
    //             Vector3 coinPos = coinObject.PositionAfter(lane);
    //             GameObject coinGo = Instantiate(coinTiles, new Vector3(coinPos.x, coinPos.y,coinPos.z+ transform.forward.z * zSpawn), Quaternion.AngleAxis(90, Vector3.right));
    //             coinGo.transform.parent = roadObject.transform;
    //         }
    //     }
    // }
}
