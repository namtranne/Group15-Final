using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform panelTransform;
    void Start()
    {
        transform.position = panelTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
