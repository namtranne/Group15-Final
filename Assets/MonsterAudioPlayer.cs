using System.Collections; // Add this namespace
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // Assign your audio clip in the Inspector
    private AudioSource audioSource; // To play the audio

    private void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = gameObject.AddComponent<AudioSource>();

        // Start the coroutine to play the audio
        StartCoroutine(PlayAudioEveryThreeSeconds());
    }

    private IEnumerator PlayAudioEveryThreeSeconds()
    {
        while (true) // Infinite loop
        {
            int index = Random.Range(0,audioClips.Length);
            audioSource.clip = audioClips[index];
            audioSource.Play(); // Play the audio clip
            yield return new WaitForSeconds(3f); // Wait for 3 seconds
        }
    }
}