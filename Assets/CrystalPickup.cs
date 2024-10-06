using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private int value = 1;

    // public GameObject pickupEffect;

    public AudioClip soundToPlay;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Player")
        {
            GameManager.instance.AddCrystals(value);
            AudioManager.instance.PlayAudio(soundToPlay);
            Destroy(gameObject);
        }
    }

}