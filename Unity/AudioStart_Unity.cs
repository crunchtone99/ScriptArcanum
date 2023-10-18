using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Start_Script : MonoBehaviour
{
    public AudioSource windSource;
    
    // Start is called before the first frame update
    void Start()
    {
        windSource = gameObject.GetComponent<AudioSource>();
        windSource.volume = 0.7f;
        windSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
