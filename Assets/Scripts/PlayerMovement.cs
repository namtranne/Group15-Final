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
    }

    private void MoveForward()
    {
        if(!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
            rb.AddForce(0, 0, forwardForce * Time.deltaTime);
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
            Debug.Log("Jump");
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
        // Raycast down to check for ground
        float rayDistance = 1.1f; // Adjust based on your player's height
        if(Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask)) {
            jumpCount = 0;
        }

        // Debug.Log("Is Grounded: " + isGrounded);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the player collides with something tagged as ground, set isGrounded to true
        if (collision.collider.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

    // private void OnCollisionExit(Collision collision)
    // {
    //     if (collision.collider.CompareTag("Ground"))
    //     {
    //         isGrounded = false;
    //     }
    // }
}
