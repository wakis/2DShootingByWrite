using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyHP : MonoBehaviour
{

    [SerializeField]
    Color MaxColor;
    [SerializeField]
    Color minColor;
    Color gradation;
    
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
        objRule = Camera.main.GetComponent<gameObj>();
        MaxHP = objRule.Boss.GetComponent<eBossMove>().MAXHP;
        penaltyTime = 0f;
        HP = MaxHP;
        lineMaker = objRule.GetComponent<addLine>();
        line = GetComponent<LineRenderer>();
        LineLengthIsX = ((line.GetPosition(1) - line.GetPosition(0)).x > (line.GetPosition(1) - line.GetPosition(0)).y ? true : false);
        MaxLineLength = (LineLengthIsX ? (line.GetPosition(1) - line.GetPosition(0)).x : (line.GetPosition(1) - line.GetPosition(0)).y) / 2f;
        gradation = MaxColor - minColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (objRule.Boss!=null) {
            HP = objRule.Boss.GetComponent<eBossMove>().getHP;
        }else
        {
            HP = 0;
        }
        set_HP_Bar();
    }

    void set_HP_Bar()
    {
        float LineLength = (HP / MaxHP) * MaxLineLength;
        if (LineLengthIsX)
        {
            if (line.GetPosition(1).x != LineLength)
            {
                line.SetPosition(0, new Vector3(-LineLength, 0f, 0f));
                line.SetPosition(1, new Vector3(LineLength, 0f, 0f));
                float percent = (1f - LineLength / MaxLineLength * 1.5f) > 0f ?
                    (1f - LineLength / MaxLineLength * 1.1f) : 0f;
                line.startColor = line.endColor = MaxColor - gradation * percent;
            }
        }
        else
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
