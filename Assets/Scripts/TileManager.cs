using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    //public GameObject[] tilesPrefabs;
    private List<Obstacle> obstableObject;
    // private Coin coinObject;
    public GameObject[] obstacleTiles;
    public GameObject coinTiles;
    public GameObject roadOject;
    public float zSpawn = 0;
    public float tileLength=26.0f;
    public int numberOfTiles=5;
    public Transform playerTransform;
    private List<GameObject> activeTile = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        obstableObject = new List<Obstacle>();
        for(int i = 0; i < obstacleTiles.Length; i++)
        {
            switch (i)
            {
                case 0:
                    obstableObject.Add( new Wall () { obstacleTiles= obstacleTiles[i] });
                    break;
                // case 1:
                //     obstableObject.Add(new FenceHigh() { obstacleTiles = obstacleTiles[i] });
                //     break;
                // case 2:
                //     obstableObject.Add(new FenceLow() { obstacleTiles = obstacleTiles[i] });
                //     break;
                // case 3:
                //     obstableObject.Add(new TrainX() { obstacleTiles = obstacleTiles[i] });
                //     break;
                // case 4:
                //     obstableObject.Add(new TrainZ() { obstacleTiles = obstacleTiles[i] });
                //     break;
            }
        }
        // coinObject = new Coin(coinTiles);
        for(int i=0;i<numberOfTiles;i++)
        {
            if (i == 0) GenerateObject(true);
            else GenerateObject();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z - tileLength>= zSpawn-(numberOfTiles*tileLength))
        {
            GenerateObject();
            DeleteTile();
        }
    }
    private void DeleteTile()
    {
        Destroy(activeTile[0]);
        activeTile.RemoveAt(0);
    }

    private void GenerateObject(bool start=false)
    {
        // bool checkX(Obstacle o)
        // {
        //     if (o is TrainX || o is FenceHigh) return true;
        //     else return false;
        // }
        GameObject roadGo = Instantiate(roadOject, transform.forward * zSpawn, Quaternion.identity);

        int numOfObsPerRow=Random.Range(1, 3);
        int[] lanes = Enumerable.Range(0, 3).OrderBy(x => Random.Range(0, 3)).Take(2).ToArray();
        for (int i = 1; i <= numOfObsPerRow; i++)
        {
            int obstacleIndex = Random.Range(0, obstableObject.Count);
            Lane lane= (Lane)lanes[i-1];
            // if (checkX(obstableObject[obstacleIndex]) && numOfObsPerRow != 1) continue;
            Vector3 obsPos = obstableObject[obstacleIndex].Position(lane);
            // Make the obstacle face in the same direction as the roadGo
            Quaternion rotationToFaceRoad = Quaternion.AngleAxis(90, roadGo.transform.up) * roadGo.transform.rotation;

            // Instantiate the object and make it face in the direction of the roadGo
            GameObject obsGo = Instantiate(obstableObject[obstacleIndex].obstacleTiles, 
                                        new Vector3(obsPos.x, obsPos.y, zSpawn), 
                                        rotationToFaceRoad);
            if (start) obsGo.SetActive(false);
            obsGo.transform.parent = roadGo.transform;
            // if (checkX(obstableObject[obstacleIndex])) break;
        }
        // if(!start) GenerateCoin(roadGo);
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
