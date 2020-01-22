using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ctrlAnim : MonoBehaviour
{
    Animator anim;
    gameObj objRule;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        objRule = Camera.main.GetComponent<gameObj>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.speed = objRule.gradationTimePer;
    }
}
