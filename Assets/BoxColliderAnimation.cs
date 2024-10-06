using UnityEngine;

public class BoxColliderAnimation : MonoBehaviour
{
    public BoxCollider boxCollider;

    private Vector3 startPosition = new Vector3(2.5f, 1.99f, 0f);
    private Vector3 middlePosition = new Vector3(2.5f, 1.99f, 31.22f);
    private Vector3 endPosition = new Vector3(2.5f, 1.99f, 59.3f);

    private Vector3 startSize = new Vector3(8.33f, 9.04f, 0f);
    private Vector3 middleSize = new Vector3(8.33f, 9.04f, 59.3f);
    private Vector3 endSize = new Vector3(8.33f, 9.04f, 0f);

    private float animationDuration = 1f; // 1 second for each phase
    private float elapsedTime = 0f;

    private enum AnimationPhase { First, Second, None }
    private AnimationPhase currentPhase = AnimationPhase.First;

    void Start()
    {
        // Ensure the BoxCollider is assigned
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        // Set initial position and size
        boxCollider.center = startPosition;
        boxCollider.size = startSize;
    }

    void Update()
    {
        if (currentPhase != AnimationPhase.None)
        {
            elapsedTime += Time.deltaTime;

            // Animate based on the current phase
            if (currentPhase == AnimationPhase.First)
            {
                AnimateBoxCollider(startPosition, middlePosition, startSize, middleSize);
                if (elapsedTime >= animationDuration)
                {
                    // Move to next phase after first phase is complete
                    elapsedTime = 0f;
                    currentPhase = AnimationPhase.Second;
                }
            }
            else if (currentPhase == AnimationPhase.Second)
            {
                AnimateBoxCollider(middlePosition, endPosition, middleSize, endSize);
                if (elapsedTime >= animationDuration)
                {
                    // Stop after second phase is complete
                    elapsedTime = 0f;
                    currentPhase = AnimationPhase.None;
                }
            }
        }
    }

    private void AnimateBoxCollider(Vector3 startPos, Vector3 endPos, Vector3 startSize, Vector3 endSize)
    {
        // Calculate the current interpolation factor
        float t = elapsedTime / animationDuration;

        // Interpolate the center position and size
        boxCollider.center = Vector3.Lerp(startPos, endPos, t);
        boxCollider.size = Vector3.Lerp(startSize, endSize, t);
    }
}
