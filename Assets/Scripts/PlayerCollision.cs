
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    public AudioSource collisionSoundEffect;

    void OnCollisionEnter(Collision colInfo)
    {
        if (colInfo.collider.tag == "Lava")
        {
            EndGame();
        }
    }

    public void EndGame() {
        movement.enabled = false;
        movement.Burn();
        FindObjectOfType<GameManager>().EndGame();
    }

}
