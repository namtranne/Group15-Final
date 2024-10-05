using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sideForce = 500f;
    public float jumpForce = 700f;
    private Vector3 originalScale;
    private int jumpCount = 0;
    public LayerMask groundMask;
    private float accelerationFactor = 0.2f;
    public float rotationSpeed = 2f;
    public float forceIncreaseRate = 10f;

    // Audio
    private AudioSource audioSource;
    private AudioSource landingSource;
    private AudioSource jumpingSource;
    public AudioClip walkingAudioClip;
    public AudioClip landingAudioClip;
    public AudioClip jumpingAudioClip;

    private float previousGroundDistance = Mathf.Infinity; // Store the previous distance to the ground

    protected Animator m_Animator;
    protected static PlayerMovement s_Instance;
    public static PlayerMovement instance { get { return s_Instance; } }

    public void Start()
    {
        this.audioSource = gameObject.GetComponent<AudioSource>();
        this.landingSource = gameObject.AddComponent<AudioSource>();
        jumpingSource = gameObject.AddComponent<AudioSource>();

        // Corrected assignment
        jumpingSource.clip = jumpingAudioClip;
        this.landingSource.clip = landingAudioClip;
        originalScale = transform.localScale; // Save the original scale of the player
    }

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
        CheckGrounded();
        PlayAudio();
    }

    private void MoveForward()
    {
        forwardForce += forceIncreaseRate * Time.deltaTime;
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
            RotatePlayer(Vector3.right);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
            RotatePlayer(Vector3.left);
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
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
        jumpingSource.Play();
        audioSource.clip = walkingAudioClip;
        jumpCount++;
    }

    private void RotatePlayer(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
        float rayDistance = 1f; // Increase the raycast distance to better detect the ground
        RaycastHit hit;

        // Perform the raycast to check for ground
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundMask))
        {
            float currentGroundDistance = hit.distance;

            // Check if the player is falling or landing
            if (currentGroundDistance < previousGroundDistance && jumpCount != 0) // Player is landing
            {
                Debug.Log("Landing");
                audioSource.clip = walkingAudioClip;
                audioSource.Play();
                Invoke("PlayLandingClip", 0.2f);
                jumpCount = 0;
            }

            // Update the previous distance for the next frame
            previousGroundDistance = currentGroundDistance;
        }
        else
        {
            // No ground detected, reset the previous distance
            previousGroundDistance = Mathf.Infinity;
        }
    }

    private void PlayAudio()
    {
        if (!audioSource.isPlaying && jumpCount == 0)
        {
            audioSource.clip = walkingAudioClip;
            audioSource.Play();
        }
        else if (jumpCount != 0)
        {
            audioSource.Stop();
        }
    }

    private void PlayLandingClip() {
        this.landingSource.Play();
    }
}
