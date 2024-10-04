using UnityEngine;

public class FallToSide : MonoBehaviour
{
    public Vector3 forceDirection = new Vector3(1, 0, 0); // Force to apply (1 unit on the X-axis)

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(forceDirection, ForceMode.Impulse); // Apply force to the side
        }
    }
}
