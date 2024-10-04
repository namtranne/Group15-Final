using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    public AudioSource collisionSoundEffect;

    void OnCollisionEnter(Collision colInfo)
    {
        if (colInfo.collider.tag == "Obstacle")
        {
            movement.enabled = false;
            // collisionSoundEffect.Play();

            if (!PlayerMovement.instance.isBossActive) // Use the class name instead of an instance
            {
                PlayerMovement.instance.TriggerBoss(); // Trigger the boss to follow the player
            }

            movement.enabled = true;
        }
    }
}
