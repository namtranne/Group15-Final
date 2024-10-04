using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Vector3 initialRotation;
    private Vector3 finalRotation;
    public float delayTime = 2.0f;
    private float timer = 0;
    void Start()
    {
        finalRotation = transform.eulerAngles;
        transform.eulerAngles = initialRotation;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Check if the lock time has passed
        if (timer >= delayTime)
        {
            transform.eulerAngles = finalRotation; 
        }
    }
}
