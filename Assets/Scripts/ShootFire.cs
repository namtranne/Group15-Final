using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFire : MonoBehaviour
{
    public GameObject fireSkill; // Prefab of the fire skill
    public float fireInterval = 2f; // Time interval in seconds
    private Coroutine shootCoroutine; // To hold the coroutine reference

    // Start is called before the first frame update
    void Start()
    {
        // Start the shooting coroutine
        shootCoroutine = StartCoroutine(ShootRoutine());
    }

    // Coroutine to repeatedly shoot fire skills
    private IEnumerator ShootRoutine()
    {
        while (true) // Infinite loop until stopped
        {
            Shoot(); // Call the Shoot method
            yield return new WaitForSeconds(fireInterval); // Wait for the specified interval
        }
    }

    // This method will spawn the fire skill at the firePoint's position
    void Shoot()
    {
        Instantiate(fireSkill, transform.position, transform.rotation);
    }

    // This method is called when the game object is disabled
    void OnDisable()
    {
        // Stop the shooting coroutine when the dropship is disabled
        if (shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null; // Clear the reference
        }
    }

    void OnEnable()
    {
        shootCoroutine = StartCoroutine(ShootRoutine());
    }
}
