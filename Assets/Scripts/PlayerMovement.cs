using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sideForce = 500f;
    public float jumpForce = 700f;
    private int jumpCount = 0;
    public LayerMask groundMask; // Layer for ground objects
    private float accelerationFactor = 0.2f;

    // Boss-related variables
    public GameObject bossPrefab; // Prefab for the boss object
    private GameObject bossInstance; // Instance of the boss
    public float bossSpawnDistance = 5f; // Distance behind the player where the boss spawns
    public float bossSpeed = 1900f; // Boss movement speed
    public bool isBossActive = false; // Tracks if the boss is currently active
    public bool isRecovering = false; // Tracks if the player is in recovery mode
    private float reducedSpeedMultiplier = 0.5f; // Multiplier for reducing player speed
    private float recoveryTime = 5f; // Time to recover after collision
    private float recoveryTimer;
    private float bossCaptureDistance = 1.5f; // Distance to trigger capture

    protected Animator m_Animator;
    protected static PlayerMovement s_Instance;
    public static PlayerMovement instance { get { return s_Instance; } }

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        s_Instance = this;
    }

    private void Update()
    {
        MoveForward();
        ProcessInput();
        CheckFallOff();
        CheckGrounded(); // Use raycasting to check if grounded

        if (isBossActive)
        {
            MoveBossTowardsPlayer();
            CheckBossCapture();
        }

        if (isRecovering)
        {
            recoveryTimer -= Time.deltaTime;
            if (recoveryTimer <= 0)
            {
                RecoverPlayer();
            }
        }
    }

    private void MoveForward()
    {
        if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            float currentForwardForce = forwardForce * (isRecovering ? reducedSpeedMultiplier : 1f);
            rb.AddForce(0, 0, currentForwardForce * Time.deltaTime);
        }
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1)
        {
            Jump();
        }
    }

    private void MoveRight()
    {
        rb.AddForce(sideForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }

    private void MoveLeft()
    {
        rb.AddForce(-sideForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
    }

    private void Jump()
    {
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        rb.AddForce(0, 0, 7000 * Time.deltaTime);
        jumpCount++; // Set grounded to false immediately after jumping
    }

    private void CheckFallOff()
    {
        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    private void CheckGrounded()
    {
        float rayDistance = 1.1f;
        if (Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask))
        {
            jumpCount = 0;
        }
    }

    public void TriggerBoss()
    {
        if (!isBossActive)
        {
            // Instantiate the boss prefab behind the player
            Vector3 spawnPosition = transform.position - new Vector3(0, 0, bossSpawnDistance);
            bossInstance = Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            
            isBossActive = true;
            isRecovering = true;
            recoveryTimer = recoveryTime;
            forwardForce *= reducedSpeedMultiplier; // Reduce player speed
        }
    }

    private void MoveBossTowardsPlayer()
    {
        if (bossInstance != null)
        {
            Vector3 directionToPlayer = (transform.position - bossInstance.transform.position).normalized;
            bossInstance.GetComponent<Rigidbody>().AddForce(directionToPlayer * bossSpeed * Time.deltaTime);
        }
    }

    private void CheckBossCapture()
    {
        if (bossInstance != null)
        {
            float distanceToBoss = Vector3.Distance(transform.position, bossInstance.transform.position);
            if (distanceToBoss < bossCaptureDistance)
            {
                FindObjectOfType<GameManager>().EndGame(); // End the game when the boss captures the player
            }
        }
    }

    private void RecoverPlayer()
    {
        isRecovering = false;
        forwardForce /= reducedSpeedMultiplier; // Restore original speed

        // Destroy the boss after recovery
        if (bossInstance != null)
        {
            Destroy(bossInstance);
        }
        
        isBossActive = false;
    }
}
