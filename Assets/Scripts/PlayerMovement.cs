using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sideForce = 500f;
    public float jumpForce = 700f;           // Normal jump force
    public float boostedJumpForce = 1200f;   // Increased jump force for the jump skill
    private Vector3 originalScale;
    private int jumpCount = 0;
    public LayerMask groundMask;
    private float accelerationFactor = 0.2f;
    public float rotationSpeed = 2f;
    public float forceIncreaseRate = 10f;
    private bool isDead = false;

    // Audio
    private AudioSource audioSource;
    private AudioSource landingSource;
    private AudioSource jumpingSource;
    private AudioSource deathSource;
    public AudioClip walkingAudioClip;
    public AudioClip landingAudioClip;
    public AudioClip jumpingAudioClip;
    public AudioClip deathAudioClip;

    private float previousGroundDistance = Mathf.Infinity; // Store the previous distance to the ground

    protected Animator m_Animator;
    protected static PlayerMovement s_Instance;
    public static PlayerMovement instance { get { return s_Instance; } }

    private bool isIntangible = false; // Track whether the player is currently intangible
    private float normalForwardForce;  // Store the normal forward force value
    private int normalLayer;           // Store the original layer of the player
    private bool jumpSkillActive = false; // Track whether the jump skill is active
    private float normalJumpForce;      // Store the original jump force

    public void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        landingSource = gameObject.AddComponent<AudioSource>();
        jumpingSource = gameObject.AddComponent<AudioSource>();
        deathSource = gameObject.AddComponent<AudioSource>();

        jumpingSource.clip = jumpingAudioClip;
        deathSource.clip = deathAudioClip;
        this.landingSource.clip = landingAudioClip;
        originalScale = transform.localScale; // Save the original scale of the player
        normalForwardForce = forwardForce;    // Save the normal forward force
        normalJumpForce = jumpForce;          // Save the normal jump force
        normalLayer = gameObject.layer;       // Save the original layer
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

        // Check if the Intangible power-up is active
        if (PowerUpManager.instance.IsPowerUpActive("Intangible"))
        {
            ActivateIntangibleEffect();
        }
        else if (isIntangible)
        {
            DeactivateIntangibleEffect();
        }

        // Check if the Jump Skill is active
        if (PowerUpManager.instance.IsPowerUpActive("Jump"))
        {
            ActivateJumpSkill();
        }
        else if (jumpSkillActive)
        {
            DeactivateJumpSkill();
        }
    }

    private void MoveForward()
    {
        forwardForce += forceIncreaseRate * Time.deltaTime;
        normalForwardForce += forceIncreaseRate * Time.deltaTime;
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
                audioSource.clip = walkingAudioClip;
                audioSource.Play();
                Invoke("PlayLandingClip", 0.2f);
                jumpCount = 0;
            }

            previousGroundDistance = currentGroundDistance;
        }
        else
        {
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

    private void PlayLandingClip()
    {
        this.landingSource.Play();
    }

    public void Burn()
    {
        if (isDead) return;
        isDead = true;
        m_Animator.SetBool("isDeath", true);
        deathSource.Play();
    }

    // Method to activate the Jump Skill
    private void ActivateJumpSkill()
    {
        if (!jumpSkillActive)
        {
            jumpForce = boostedJumpForce; // Set to the boosted jump force
            jumpSkillActive = true;       // Track that the jump skill is active
        }
    }

    // Method to deactivate the Jump Skill
    private void DeactivateJumpSkill()
    {
        jumpForce = normalJumpForce; // Revert to the normal jump force
        jumpSkillActive = false;     // Track that the jump skill is no longer active
    }

    // Method to activate Intangible power-up effect
    private void ActivateIntangibleEffect()
    {
        if (!isIntangible)
        {
            Debug.Log("Intangible Power-Up Activated: Boosting speed and changing layer.");
            forwardForce = normalForwardForce * 10; // Increase the speed
            gameObject.layer = LayerMask.NameToLayer("Intangible"); // Change the layer
            isIntangible = true; // Track that the player is intangible
        }
        else
        {
            int powerUpTime = (int)PowerUpManager.instance.GetPowerUpTime("Intangible");
            if (powerUpTime >= 5)
            {
                forwardForce = normalForwardForce * 10;
            }
            else if (powerUpTime <= 1)
            {
                forwardForce = normalForwardForce * 2;
            }
            else
            {
                forwardForce = normalForwardForce * powerUpTime * 2;
            }
        }
    }

    // Method to deactivate Intangible power-up effect
    private void DeactivateIntangibleEffect()
    {
        forwardForce = normalForwardForce; // Revert to normal speed
        gameObject.layer = normalLayer;         // Revert to the original layer
        isIntangible = false;                   // Track that the player is no longer intangible
    }
}
