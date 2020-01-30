using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStatus : MonoBehaviour
{
    [SerializeField]
    PointGradation point1;
    [SerializeField]
    GameObject[] popObj;
    int step;
    GameObject boss;

    [SerializeField]
    GameObject[] anyEnemy;

    gameObj objRule;
    private void Awake()
    {
        step = 0;
        objRule = Camera.main.GetComponent<gameObj>();
        boss = objRule.Boss;
        boss.GetComponent<objStatusRenewal>().enabled = false;
        foreach(var obj in anyEnemy)
        {
            obj.SetActive(false);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!popObj[step].activeSelf)
        {
            popObj[step].SetActive(true);
            if (step>0)
            {
                popObj[step-1].SetActive(false);
            }
        }

        switch (step)
        {
            case 0:
                if (point1.onTriggerInObj)
                {
                    step += 1;
                    point1.onTriggerInObj = false;
                }
                break;
            case 1:
                if (objRule.player.GetComponent<player>().getNownowBullet.GetComponent<linebullet>()||Input.GetButtonUp("Linewrite"))
                {
                    step += 1;
                }
                break;
            case 2:
                if (boss == null) return;
                if (!boss.GetComponent<objStatusRenewal>().enabled)
                {
                    boss.GetComponent<objStatusRenewal>().enabled = true;
                    foreach (var obj in anyEnemy)
                    {
                        obj.SetActive(true);
                    }
                }
                if (Input.GetButton("Shot"))
                {
                    //step += 1;
                    //objRule.gameclear = true;
                }
                break;
            case 3:
                //objRule.gameclear = true;
                break;
            case 5:
                break;
        }
    }

    void stepup(int lp)
    {

    }
}
