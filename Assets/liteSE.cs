using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class liteSE : MonoBehaviour
{
    AudioSource Audio;
    bool OnGame;
    gameObj objRule;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
        Audio.Pause();
        OnGame = Camera.main.GetComponent<gameObj>();
        if (OnGame)
        {
            objRule= Camera.main.GetComponent<gameObj>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OnGame) {
            if (objRule.onConcentration && !Audio.isPlaying)
            {
                Audio.Play();
            }
            if (!objRule.onConcentration && Audio.isPlaying)
            {
                Audio.Pause();
            }
        }else
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
}
