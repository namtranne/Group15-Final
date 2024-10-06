using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGenerator : MonoBehaviour
{
    public List<GameObject> treePrefabs; // List of tree prefabs to randomly choose from
    public List<Transform> treePositions;  // List of positions where trees will be generated
    
    private List<GameObject> generatedTrees = new List<GameObject>(); // To store instantiated tree objects

    // Start is called before the first frame update
    void Start()
    {
        if (treePositions.Count > treePrefabs.Count)
        {
            Debug.LogError("Not enough tree prefabs to fill all positions!");
            return;
        }

        // Create a copy of the treePrefabs list to avoid modifying the original list in the inspector
        List<GameObject> availableTreePrefabs = new List<GameObject>(treePrefabs);

        // Loop through each position and instantiate a unique tree
        foreach (Transform transform in treePositions)
        {
            // Randomly select a tree prefab from the available list
            int randomIndex = Random.Range(0, availableTreePrefabs.Count);
            GameObject selectedTreePrefab = availableTreePrefabs[randomIndex];

            // Instantiate the selected tree at the current position
            GameObject obj = Instantiate(selectedTreePrefab, transform.position, Quaternion.identity);

            // Store the generated tree object
            generatedTrees.Add(obj);

            // Remove the selected tree prefab from the list to prevent duplicates
            availableTreePrefabs.RemoveAt(randomIndex);
        }
    }

    // This method is called when the GameObject this script is attached to is destroyed
    private void OnDestroy()
    {
        // Destroy all generated trees when the object containing this script is destroyed
        foreach (GameObject tree in generatedTrees)
        {
            if (tree != null)
            {
                Destroy(tree);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // You can add additional logic here if needed
    }
}
