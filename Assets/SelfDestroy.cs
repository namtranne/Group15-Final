using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float destroyTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Destroy() {
        Destroy(gameObject);
    }
}
