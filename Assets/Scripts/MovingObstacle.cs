using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    private float speed = 10f; // Speed of movement
    private GameObject player; // Reference to the player object
    private bool isMoving = false; // Flag to track if obstacle should move

    public float triggerDistance = 20f; // Distance at which obstacle starts moving

    // Start is called before the first frame update
    void Start()
    {
        // Find the player object by tag
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is within the trigger distance
        if (Vector3.Distance(transform.position, player.transform.position) <= triggerDistance)
        {
            isMoving = true;
        }

        // If the obstacle should move, move it towards the player
        if (isMoving)
        {
            // Move the obstacle toward the player along the z-axis
            transform.position = Vector3.MoveTowards(transform.position, 
                                                     new Vector3(transform.position.x, transform.position.y, player.transform.position.z), 
                                                     speed * Time.deltaTime);

            // Check if the obstacle has passed over the player
            if (transform.position.z <= player.transform.position.z)
            {
                // Destroy the obstacle after it passes the player
                Destroy(gameObject);
            }
        }
    }
}
