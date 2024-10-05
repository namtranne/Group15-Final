using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;             // Reference to the player
    public Vector3 offset;               // Offset from the player during normal movement
    public float delayTime = 2f;         // Time to wait before camera starts following the player
    private float timer = 0f;            // Timer to keep track of time passed
    private bool isLocked = true;        // Flag to keep the camera locked initially
    public Vector3 targetRotation;       // Target rotation in Euler angles for when the camera is unlocked
    public Vector3 targetPosition;       // Target position for when the camera is unlocked

    // Update is called once per frame
    void Update()
    {
        // Count the time
        timer += Time.deltaTime;

        // Check if the lock time has passed
        if (timer >= delayTime)
        {
            isLocked = false;  // Unlock the camera
            transform.eulerAngles = targetRotation;  // Convert Vector3 to Quaternion
            transform.position = targetPosition;
        }

        // Once the camera is unlocked, follow the player with the specified offset
        if (!isLocked)
        {
            transform.position = player.position + offset;
        }

    }
}
