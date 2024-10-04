
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
            EndGame();
        }
    }

    public void Hit() {
        movement.enabled = false;
        Invoke("EndGame", 1);
    }

    private void EndGame() {
        FindObjectOfType<GameManager>().EndGame();
    }
}
