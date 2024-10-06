using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    private AudioSource source;
    public AudioClip audioClip;
    public float delayStart;
    public float delayStop;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = audioClip;
        Invoke("Play", delayStart);
        Invoke("Stop", delayStop);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play() {
        source.Play();
    }

    void Stop() {
        source.Stop();
    }
}
