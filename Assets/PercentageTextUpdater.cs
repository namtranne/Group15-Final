using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI components
using System.Collections;
using UnityEngine.SceneManagement;

public class PercentageTextUpdater : MonoBehaviour
{
    private Text percentageText; // The UI Text element you want to update
    public float duration = 5f; // Time taken to go from 0% to 100%

    private void Start()
    {
        // Start the coroutine that updates the text
        percentageText = gameObject.GetComponent<Text>();
        StartCoroutine(UpdatePercentageOverTime());
    }

    IEnumerator UpdatePercentageOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the percentage (0 to 100) based on elapsed time
            float percentage = Mathf.Lerp(0, 100, elapsedTime / duration);
            percentageText.text = Mathf.RoundToInt(percentage) + "%";

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait until the next frame
            yield return null;
        }

        // Ensure it ends exactly at 100%
        percentageText.text = "100%";
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
