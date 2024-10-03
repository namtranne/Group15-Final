using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 2000f;
    public float sideForce = 500f;
    public float jumpForce = 700f; 
    public bool isGrounded = true; 

    private float accelerationFactor = 0.2f;

    
    protected Animator m_Animator;
    protected static PlayerMovement s_Instance;
    public static PlayerMovement instance { get { return s_Instance; } }

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        s_Instance = this;
    }

    void Update()
    {
        MoveForward();
        ProcessInput();
        CheckFallOff();

        
    }

    void FixUpdate()
    {
        forwardForce = Time.deltaTime * accelerationFactor;
    }

    private void MoveForward()
    {
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);
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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
        Debug.Log("Jump 2");
        rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        isGrounded = false; // Set grounded to false immediately after jumping
    }

    private void CheckFallOff()
    {
        if (rb.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // If the player collides with something tagged as ground, set isGrounded to true
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
