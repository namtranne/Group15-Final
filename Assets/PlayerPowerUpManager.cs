using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUpManager : MonoBehaviour
{
    public GameObject shieldEffect;     // Effect for the "Shield" power-up
    public GameObject intangibleEffect; // Effect for the "Intangible" power-up
    public GameObject jumpEffect;       // Effect for the "Jump Boost" power-up

    void Start()
    {
        // Ensure that all effects are initially disabled
        DisableAllEffects();
    }

    void Update()
    {
        // Check power-up statuses every frame and enable/disable the corresponding effects
        HandlePowerUpEffects();
    }

    // Method to enable or disable effects based on active power-ups
    private void HandlePowerUpEffects()
    {
        // Check if the "Shield" power-up is active
        if (PowerUpManager.instance.IsPowerUpActive("Shield"))
        {
            shieldEffect.SetActive(true); // Enable shield effect
        }
        else
        {
            shieldEffect.SetActive(false); // Disable shield effect
        }

        // Check if the "Intangible" power-up is active
        if (PowerUpManager.instance.IsPowerUpActive("Intangible"))
        {
            intangibleEffect.SetActive(true); // Enable intangible effect
        }
        else
        {
            intangibleEffect.SetActive(false); // Disable intangible effect
        }

        // Check if the "Jump Boost" power-up is active
        if (PowerUpManager.instance.IsPowerUpActive("Jump"))
        {
            jumpEffect.SetActive(true); // Enable jump boost effect
        }
        else
        {
            jumpEffect.SetActive(false); // Disable jump boost effect
        }
    }

    // Method to disable all effects (called initially to reset)
    private void DisableAllEffects()
    {
        shieldEffect.SetActive(false);
        intangibleEffect.SetActive(false);
        jumpEffect.SetActive(false);
    }
}
