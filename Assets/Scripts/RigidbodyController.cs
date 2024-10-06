using UnityEngine;

public class RigidbodyController : MonoBehaviour
{
    private GameObject player;  // Reference to the player object
    public float activationDistance = 5f;  // Distance threshold to activate Rigidbody

    private Rigidbody rb;
    private bool isRigidbodyActivated = false;
    private AudioSource audioSource;
    public AudioClip audioClip;

    public float delayTime = 0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No GameObject with tag 'Player' found.");
        }

        audioSource = gameObject.GetComponent<AudioSource>();
        if(audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.clip = audioClip;

        rb = GetComponent<Rigidbody>();

        // Initially disable the Rigidbody
        DisableRigidbody();
    }

    void Update()
    {
        // Check the distance between this object and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the object is within the activation distance and Rigidbody is not activated
        if (distanceToPlayer <= activationDistance && !isRigidbodyActivated)
        {
            EnableRigidbody();
            PlayAudioWithDelay();
        }
    }

    void DisableRigidbody()
    {
        if (rb != null)
        {
            rb.isKinematic = true;  // Disable physics
            rb.velocity = Vector3.zero;  // Reset velocity
            rb.angularVelocity = Vector3.zero;  // Reset rotation velocity
        }
    }

    void EnableRigidbody()
    {
        if (rb != null)
        {
            rb.isKinematic = false;  // Enable physics
            isRigidbodyActivated = true;  // Prevent multiple activations
        }
    }

    void PlayAudioWithDelay() {
        Invoke("PlayAudio", delayTime);
    }

    void PlayAudio() {
        audioSource.Play();
    }

}
