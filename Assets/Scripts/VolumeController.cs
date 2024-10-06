using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the Slider in the UI
    public AudioSource audioSource; // Reference to the AudioSource component

    void Start()
    {
        // Optionally, load saved volume preference from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f); // Default to 50% volume
        audioSource.volume = savedVolume;
        volumeSlider.value = savedVolume; // Set the slider to the saved value

        // Add listener to the slider to detect changes
        volumeSlider.onValueChanged.AddListener(UpdateVolume);
    }

    // Update the AudioSource volume when slider value changes
    public void UpdateVolume(float value)
    {
        audioSource.volume = value; // Set AudioSource volume
        PlayerPrefs.SetFloat("MusicVolume", value); // Save the volume level
    }
}
