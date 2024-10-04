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
    public float rotationSpeed = 2f; // Speed for smooth rotation
    public float forceIncreaseRate = 10f; // How fast the forward force increases over time

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
        // Increase forwardForce gradually over time
        forwardForce += forceIncreaseRate * Time.deltaTime;

        rb.AddForce(0, 0, forwardForce * Time.deltaTime);
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
            RotatePlayer(Vector3.right); // Rotate to face right
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
            RotatePlayer(Vector3.left); // Rotate to face left
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

    private void RotatePlayer(Vector3 direction)
    {
        // Find the target rotation based on the movement direction
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        // Smoothly rotate towards the target direction
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
        float rayDistance = 1.1f;
        if (Physics.Raycast(transform.position, Vector3.down, rayDistance, groundMask))
        {
            jumpCount = 0;
        }
    }
}
