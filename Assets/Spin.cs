using UnityEngine;

public class Spin : MonoBehaviour
{
    public float spinSpeed = 100f;  // Speed of the spin

    void Update()
    {
        // Rotate the object around its Y axis at the speed defined by spinSpeed
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime);
    }
}
