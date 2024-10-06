using UnityEngine;

public class MonsterFollowPlayer : MonoBehaviour
{
    public float chaseSpeed = 5f; // Speed at which the monster follows the player
    public float attackRange = 1.5f; // Distance from which the monster attacks the player
    public float safeDistance = 5f; // Distance the monster tries to keep from the player
    public float attackCooldown = 1f; // Time between attacks
    private float lastAttackTime = 0f;
    private GameObject playerObject;
    private Transform player; // Reference to the player's transform
    private Rigidbody playerRb; // Player's Rigidbody to monitor their velocity
    private bool isAttacking = false; // Tracks if the monster is in attacking mode
    private Animator m_Animator; // Monster's animator for attack animations
    private AudioSource attackSource;
    public AudioClip attackClip;
    public float attackAudioDelay;

    private void Start()
    {
        // Find the player using the Player tag
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerRb = playerObject.GetComponent<Rigidbody>(); // Get the player's Rigidbody
        }

        // Initialize the animator
        m_Animator = GetComponent<Animator>();
        this.attackSource = gameObject.AddComponent<AudioSource>();
        attackSource.clip = attackClip;
    }

    private void Update()
    {
        // Ensure the player was found before continuing
        if (player == null)
        {
            Debug.LogWarning("Player not found. Make sure the player is tagged 'Player'.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Always follow or monitor the player based on distance and speed
        FollowPlayer(distanceToPlayer);
    }

    private void FollowPlayer(float distanceToPlayer)
    {
        // Check if player is moving fast
        bool isPlayerFast = playerRb.velocity.magnitude > 2f; // Threshold for player speed

        // Monster maintains distance if player is fast and only moves if player slows down
        if (isPlayerFast && distanceToPlayer < safeDistance)
        {
            // Maintain distance from the player
            isAttacking = false;
            MaintainDistance();
        }
        else if (distanceToPlayer > attackRange)
        {
            // Player is slow or far away, chase the player
            isAttacking = false;
            MoveTowardsPlayer();
        }
        else
        {
            // Player is within attack range, stop moving and attack
            AttackPlayer();
        }
    }

    private void MaintainDistance()
    {
        // Monster doesn't move, waits for the player to slow down
        if (m_Animator != null)
        {
            m_Animator.SetBool("isAttacking", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move towards the player at a constant speed
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        // Optionally rotate the monster to face the player
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    private void AttackPlayer()
    {
        if (!isAttacking && Time.time - lastAttackTime > attackCooldown)
        {
            isAttacking = true;
            playerObject.GetComponent<PlayerCollision>().EndGame();
            PlayAudioDelay();
            
            lastAttackTime = Time.time;

            // Trigger attack animation
            if (m_Animator != null)
            {
                m_Animator.SetBool("isAttacking", true);
            }

            // Perform the attack logic here (e.g., reducing the player's health)
            Debug.Log("Monster is attacking the player!");
        }
        else
        {
            m_Animator.SetBool("isAttacking", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range and safe distance in the Unity editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, safeDistance);
    }

    void PlayAudioDelay() {
        Invoke("PlayAudio", attackAudioDelay);
    }


    void PlayAudio() {
        attackSource.Play();
    }
}
