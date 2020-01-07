using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eBossMove : objStatusDefault
{
    float adTrans,sign,moveSpeed;
    float[] wait = new float[2];
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Renewal(GameObject obj)//切り替え時
    {

    }

    public override int Setting(GameObject obj)//ゲーム開始時
    {
        objRule = Camera.main.GetComponent<gameObj>();
        speed = obj.GetComponent<objStatusRenewal>().eStatus.speed;
        coolTime = -1f;
        setCoolTime = obj.GetComponent<objStatusRenewal>().eStatus.setCoolTime;
        wait[0] = objRule.ScreenSize[0].y - obj.transform.position.y;
        wait[1] = objRule.ScreenSize[1].y - obj.transform.position.y;
        return 1;
    }

    public override int Stay(GameObject obj)//待機状態
    {
        Debug.Log(objRule.scrollSpeed);
        obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
        if (obj.transform.position.x < objRule.ScreenSize[1].x*0.9f) return 1;
        return 0;
    }

    public override int Play(GameObject obj)//画面内行動
    {

        Debug.Log(adTrans);
        if (adTrans>0.1f)
        {
            obj.transform.position += new Vector3(0, moveSpeed * sign*Time.deltaTime, 0);
            adTrans -= moveSpeed * Time.deltaTime;
        }
        else
        {
            if (objRule.ScreenSize[0].y - obj.transform.position.y+3f > 0f)
            {
                wait[0] = 0f;
            }else
            {
                wait[0] = objRule.ScreenSize[0].y - obj.transform.position.y + 3f;
            }
            if(objRule.ScreenSize[1].y - obj.transform.position.y-3f < 0f)
            {
                wait[1] = 0f;
            }
            else
            {
                wait[1] = objRule.ScreenSize[1].y - obj.transform.position.y - 3f;
            }
            sign = Random.Range(wait[0], wait[1]);
            moveSpeed = Random.Range(1f, Mathf.Abs(sign - obj.transform.position.y));
            adTrans = Mathf.Abs(sign);
            sign /= adTrans;
            

        }
        return 0;
    }
    public override void Loss(GameObject obj)//消失時
    {

    }
}
