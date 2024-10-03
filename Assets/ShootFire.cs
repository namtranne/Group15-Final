using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootFire : MonoBehaviour
{
    public GameObject fireSkill; // Prefab of the fire skill
    public float fireInterval = 2f; // Time interval in seconds

    // Start is called before the first frame update
    void Start()
    {
        // Repeatedly invoke the Shoot method every 2 seconds
        InvokeRepeating("Shoot", fireInterval, fireInterval);
    }

    // This method will spawn the fire skill at the firePoint's position
    void Shoot()
    {
        Instantiate(fireSkill, transform.position, transform.rotation);
    }
}
