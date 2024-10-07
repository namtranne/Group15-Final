using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    // Reference to the player's script that contains the shield logic
    private PlayerMovement player;

    // This will be called when the object collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null && PowerUpManager.instance.IsPowerUpActive("Shield")) // Assumes HasShieldSkill() method exists
            {
                Destroy(gameObject.GetComponent<Collider>());
            }
        }
    }
}
