using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineHP : MonoBehaviour
{
    [SerializeField]
    Color MaxColor;
    [SerializeField]
    Color minColor;
    [SerializeField]
    float MaxHP;
    float HP;

    gameObj objRule;
    addLine lineMaker;

    float penaltyTime;
    // Start is called before the first frame update
    void Start()
    {
        penaltyTime = 0f;
        HP = MaxHP;
        objRule = Camera.main.GetComponent<gameObj>();
        lineMaker = objRule.GetComponent<addLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP >= 0f)
        {
            if (!lineMaker.canAddLine)
            {
                lineMaker.canAddLine = !lineMaker.canAddLine;
            }
            hpManagement();
        }else
        {
            penaltyTime += Time.deltaTime;
            if (lineMaker.canAddLine)
            {
                lineMaker.canAddLine = !lineMaker.canAddLine;
            }
            if (penaltyTime>MaxHP/3)
            {
                HP = penaltyTime = 0f;
            }
        }
        Debug.Log(HP);
    }

    void hpManagement()
    {
        if (objRule.onConcentration)
        {
            if (objRule.gradationTimePer > objRule.getTimePer)
            {
                HP -= Time.deltaTime * objRule.getTimePer / objRule.gradationTimePer;
            }
            else
            {
                HP -= Time.deltaTime;
            }
        }
        else if (HP < MaxHP)
        {
            if (objRule.gradationTimePer < 1f)
            {
                HP += Time.deltaTime * objRule.gradationTimePer/2f;
            }
            else
            {
                HP += Time.deltaTime/2f;
            }
        }
        else if (HP > MaxHP)
        {
            HP = MaxHP;
        }
    }

}
