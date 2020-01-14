using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liteSE : MonoBehaviour
{
    AudioSource Audio;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        Audio.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Audio.Play();
        }
        if (Input.GetMouseButtonUp(0))
        {
            Audio.Pause();
        }
    }
}
