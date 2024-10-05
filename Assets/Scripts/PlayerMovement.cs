using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sideForce = 500f;
    public float jumpForce = 700f;
    public float slideForce = 1000f; // Extra forward force when sliding
    public float slideDuration = 1f; // How long the slide lasts
    public float slideCooldown = 2f; // Cooldown time between slides
    public bool isSliding = false; // To track if the player is sliding
    private float slideTimer = 0f;
    private float slideCooldownTimer = 0f;
    private Vector3 originalScale;
    private int jumpCount = 0;
    public LayerMask groundMask;
    private float accelerationFactor = 0.2f;
    public float rotationSpeed = 2f;
    public float forceIncreaseRate = 10f;

    // Audio
    private AudioSource audioSource;
    public AudioClip walkingAudioClip;
    public AudioClip jumpingAudioClip;
    public AudioClip slidingAudioClip; // New sound for sliding

    protected Animator m_Animator;
    protected static PlayerMovement s_Instance;
    public static PlayerMovement instance { get { return s_Instance; } }

    public void Start()
    {
        this.audioSource = gameObject.GetComponent<AudioSource>();
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
        HandleSliding(); // Update sliding status
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

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 1)
        {
            Jump();
        }

        // Trigger slide when pressing Left Shift and not already sliding
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ) && !isSliding && slideCooldownTimer <= 0)
        {
            StartSlide();
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
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = jumpingAudioClip;
        newAudioSource.Play();
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
        float rayDistance = 0.16f;
        if (Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask) && jumpCount != 0)
        {
            audioSource.clip = walkingAudioClip;
            audioSource.Play();
            jumpCount = 0;
        }
    }

    private void PlayAudio()
    {
        if (!audioSource.isPlaying && jumpCount == 0 && !isSliding)
        {
            audioSource.clip = walkingAudioClip;
            audioSource.Play();
        }
        else if (jumpCount != 0 || isSliding)
        {
            audioSource.Stop();
        }
    }

    // Start sliding
    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        slideCooldownTimer = slideCooldown;

        // Lower the player's height (simulate crouching)
        transform.localScale = new Vector3(originalScale.x, originalScale.y * 0.5f, originalScale.z);

        // Apply a force to slide forward
        rb.AddForce(0, 0, slideForce, ForceMode.Impulse);

        // Play sliding sound
        // audioSource.clip = slidingAudioClip;
        // audioSource.Play();
    }

    // Handle sliding duration and reset
    private void HandleSliding()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                EndSlide();
            }
        }

        // Handle slide cooldown
        if (slideCooldownTimer > 0)
        {
            slideCooldownTimer -= Time.deltaTime;
        }
    }

    // End the slide and reset player scale
    private void EndSlide()
    {
        isSliding = false;
        transform.localScale = originalScale; // Reset to original height
        audioSource.Stop();
    }
}
