using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgmSetter : MonoBehaviour
{
    [SerializeField]
    float overTime;
    [SerializeField]
    float setVolume;
    [SerializeField]
    AudioClip[] BGM = new AudioClip[3];
    bool[] BGMset = { false, false, false };
    AudioSource Audio;

    int number;
    gameObj objRule;
    private void Awake()
    {
        if (GetComponent<AudioSource>())
        {
            Audio = GetComponent<AudioSource>();
        }else
        {
            this.enabled = false;
            return;
        }
        for (int lp=0;lp< BGMset.Length;lp++)
        {
            if (!BGM[lp])
            {
                this.enabled = false;
                Audio.enabled = false;
                return;
            }
            BGMset[lp] = false;
        }
        number = 0;
        Audio.clip = BGM[number];
    }
    // Start is called before the first frame update
    void Start()
    {
        objRule= Camera.main.GetComponent<gameObj>();
        Audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!objRule.gameclear) {
            if (objRule.time < overTime) {
                Audio.volume = setVolume + (1f - setVolume) * objRule.time / overTime;
            }
            if (objRule.time < overTime / 2f && number != 0)
            {
                number = 0;
            }
            else if (objRule.time > overTime / 2f && objRule.time < overTime && number != 1)
            {
                number = 1;
            }
            else if (objRule.time >= overTime && number != 2)
            {
                number = 2;
                Audio.volume = 1f;
            }
        }else
        {
            number = 1;
        }

        if (Audio.clip.name != BGM[number].name)
        {
            setBGM();
        }
    }
    void setBGM()
    {
        float mTime = Audio.time;
        Audio.clip = BGM[number];
        Audio.time = mTime;
        Audio.Play();
    }
}
