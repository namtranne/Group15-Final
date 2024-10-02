using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variables for the Rigidbody and movement forces
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sideForce = 500f;
    public float jumpForce = 700f; 
    public bool isGrounded = true; 

    
    protected Animator m_Animator;

    protected static PlayerMovement s_Instance;
    public static PlayerMovement instance { get { return s_Instance; } }

    // Called when the script instance is being loaded
    void Awake()
    {
        m_Animator = GetComponent<Animator>();
        s_Instance = this;
    }

    void FixedUpdate()
    {
        MoveForward();

        ProcessInput();

        CheckFallOff();

        
    }

    // Method to add forward force to the player
    private void MoveForward()
    {
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);
    }

    // Method to process side movement input and jump input
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
        isGrounded = false;
    }

    private void CheckFallOff()
    {
        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // If the player is colliding with something tagged as ground, set isGrounded to true
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
