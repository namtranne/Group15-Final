
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerMovement movement;
    public AudioSource collisionSoundEffect;
    public AudioClip powerUpSound;

    void OnCollisionEnter(Collision colInfo)
    {
        if (colInfo.collider.tag == "Lava")
        {
            EndGame();
        }
        else if(colInfo.collider.tag == "PowerUp") 
        {    
            PowerUpManager.instance.CollectPowerUp(colInfo.collider.name);
            AudioManager.instance.PlayAudio(powerUpSound);
            Destroy(colInfo.collider.gameObject);
        }
    }

    public void EndGame() {
        if(PowerUpManager.instance.IsPowerUpActive("Shield")) {
            return;
        }
        movement.enabled = false;
        movement.Burn();
        FindObjectOfType<GameManager>().EndGame();
    }

}
