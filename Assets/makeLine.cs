using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeLine : MonoBehaviour
{
    gameObj objRule;
    // Start is called before the first frame update
    void Start()
    {
        objRule = Camera.main.GetComponent<gameObj>();
    }

    // Update is called once per frame
    void Update()
    {
        objRule.onConcentration = true;
        objRule.gradationTimePer = 0.01f;
        objRule.GetComponent<addLine>().HP = objRule.GetComponent<addLine>().MaxHP;
        if (Input.GetMouseButtonUp(0))
        {
            //objRule.onConcentration = false;
            Destroy(gameObject);
        }
    }
}
