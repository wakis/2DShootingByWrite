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

    //[SerializeField]
    //float MaxHP;
    //float HP;

    gameObj objRule;
    addLine lineMaker;
    LineRenderer line;

    float MaxLineLength;
    bool LineLengthIsX;

    float penaltyTime;

    bool overHeat;
    // Start is called before the first frame update
    void Start()
    {
        overHeat = false;
        penaltyTime = 0f;
        //HP = MaxHP;
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
        if (lineMaker.HP<=0f)
        {
            lineMaker.HP = 0f;
            if (lineMaker.canAddLine)
            {
                lineMaker.canAddLine = !lineMaker.canAddLine;
            }
        }
        Debug.Log("zen"+lineMaker.canAddLine);
        if (lineMaker.HP > 0f&& lineMaker.canAddLine)
        {
            hpManagement();
        }else
        {
            penaltyTime += Time.deltaTime;
            if (penaltyTime> 3f)
            {
                lineMaker.HP = penaltyTime = 0.01f;
                if (!lineMaker.canAddLine)
                {
                    lineMaker.canAddLine = !lineMaker.canAddLine;
                }
            }
        }
        set_HP_Bar();
        Debug.Log(lineMaker.canAddLine);
    }

    void hpManagement()
    {
        if (objRule.onConcentration)
        {
            if (objRule.gradationTimePer > objRule.getTimePer)
            {
                lineMaker.HP -= Time.deltaTime * objRule.getTimePer / objRule.gradationTimePer;
            }
            else
            {
                lineMaker.HP -= Time.deltaTime;
            }
        }else if (lineMaker.HP <= 0f)
        {
        }
        else if (lineMaker.HP < lineMaker.MaxHP)
        {
            if (objRule.gradationTimePer < 1f)
            {
                lineMaker.HP += Time.deltaTime * objRule.gradationTimePer/2f;
            }
            else
            {
                lineMaker.HP += Time.deltaTime/2f;
            }
        }
        else if (lineMaker.HP > lineMaker.MaxHP)
        {
            lineMaker.HP = lineMaker.MaxHP;
        }
    }


    void set_HP_Bar()
    {
        float LineLength = lineMaker.HP / lineMaker.MaxHP * MaxLineLength;
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
