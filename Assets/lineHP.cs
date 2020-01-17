using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineHP : MonoBehaviour
{
    [SerializeField]
    Color MaxColor;
    [SerializeField]
    Color minColor;
    Color gradation;

    [SerializeField]
    float MaxHP;
    float HP;

    gameObj objRule;
    addLine lineMaker;
    LineRenderer line;

    float MaxLineLength;
    bool LineLengthIsX;

    float penaltyTime;
    // Start is called before the first frame update
    void Start()
    {
        penaltyTime = 0f;
        HP = MaxHP;
        objRule = Camera.main.GetComponent<gameObj>();
        lineMaker = objRule.GetComponent<addLine>();
        line = GetComponent<LineRenderer>();
        LineLengthIsX = ((line.GetPosition(1) - line.GetPosition(0)).x > (line.GetPosition(1) - line.GetPosition(0)).y ? true : false);
        MaxLineLength = (LineLengthIsX ? (line.GetPosition(1) - line.GetPosition(0)).x : (line.GetPosition(1) - line.GetPosition(0)).y) / 2f;
        gradation = MaxColor - minColor;
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
        set_HP_Bar();
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


    void set_HP_Bar()
    {
        float LineLength = HP / MaxHP * MaxLineLength;
        if (LineLengthIsX)
        {
            if (line.GetPosition(1).x!= LineLength)
            {
                line.SetPosition(0,new Vector3(-LineLength, 0f, 0f));
                line.SetPosition(1, new Vector3(LineLength, 0f, 0f));
                float percent = (1f - LineLength / MaxLineLength * 1.5f) > 0f ?
                    (1f - LineLength / MaxLineLength * 1.1f) : 0f;
                line.startColor = line.endColor = MaxColor - gradation * percent;
            }
        }else
        {
            if (line.GetPosition(1).y != LineLength)
            {
                line.SetPosition(0, new Vector3(0f, LineLength, 0f));
                line.SetPosition(1, new Vector3(0f, -LineLength, 0f));
                line.startColor = line.endColor = MaxColor - gradation * (LineLength / MaxLineLength);
            }
        }
    }
}
