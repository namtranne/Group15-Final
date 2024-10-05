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

    public Vector3 slidingOffset = new Vector3(0, 2, -4);  // Offset to use when player is sliding
    public float smoothTransitionSpeed = 5f;               // Speed at which the camera transitions to sliding offset

    private bool isSliding = false;      // To track if the player is sliding
    public PlayerMovement playerMovement;

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
            isSliding = playerMovement.isSliding;
            Vector3 desiredOffset = isSliding ? slidingOffset : offset;
            transform.position = player.position + desiredOffset;
        }

    }
}
