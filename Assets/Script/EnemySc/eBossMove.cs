using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eBossMove : objStatusDefault
{
    float adTrans,sign,moveSpeed;
    float[] wait = new float[2];
    [SerializeField]
    GameObject[] bullets = new GameObject[8];
    float[] coolShotEnemy = new float[5];
    bool shotflag;
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
        shotflag = false;
        return 1;
    }

    public override int Stay(GameObject obj)//待機状態
    {

        obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
        if (obj.transform.position.x < objRule.ScreenSize[1].x*0.8f) return 1;
        return 0;
    }

    public override int Play(GameObject obj)//画面内行動
    {


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
        Attack_Patterner(0);
        return 0;
    }
    public override void Loss(GameObject obj)//消失時
    {

    }
    protected void GeneratorNenemy(GameObject obj, float transY)
    {
        var o = Instantiate(obj, transform.position + new Vector3(-1f, transY, 0f), Quaternion.Euler(Vector3.zero));
        o.name = obj.name;
    }
    protected void Attack_Patterner(int n)
    {
        bool endf = false;
        switch (n)
        {
            case 0:
                endf = shotEnemy();
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
            case 15:
                break;
            default:
                break;
        }
        if (n == -1) shotflag = false;
    }

    bool shotDefault()
    {
        if (coolTime < 0f)
        {
            int spot = (int)Random.Range(-2, 2)*2;
            GeneratorNenemy(bullets[1], spot);
            coolTime = 0.3f;
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
        return false;
    }

    bool shotEnemy()
    {
        if (!shotflag)
        {
            for (int lp = 0; lp < coolShotEnemy.Length; lp++) coolShotEnemy[lp] = Random.Range(0.1f, 1);
            shotflag = !shotflag;
        }
        for (int lp = -2; lp <= 2; lp++)
        {
            Debug.Log(coolShotEnemy[lp + 2]);
            if (coolShotEnemy[lp + 2] < 0f && coolShotEnemy[lp + 2] > -10f)
            {
                GeneratorNenemy(bullets[1], lp);
                coolShotEnemy[lp + 2] = -10f;
            }
            else
            {
                coolTime -= Time.deltaTime;
            }
        }
        if ((coolShotEnemy[0] < -10f && coolShotEnemy[1] < -10f && coolShotEnemy[2] < -10f &&
     coolShotEnemy[3] < -10f && coolShotEnemy[4] < -10f)) return true;
        return false;
    }
}
