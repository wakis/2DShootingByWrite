using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eBossMove : objStatusDefault
{
    public int MAXHP = 400;
    int hp;
    public int getHP{get{ return hp; }}
    int n;
    float adTrans,sign,moveSpeed;
    float turnTime;
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
        n = 0;
        hp = MAXHP;
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
        Attack_Patterner();
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
    protected void Attack_Patterner()
    {
        turnTime += Time.deltaTime;
        switch (n)
        {
            case 0:
                n = shotEnemy();
                break;
            case 1:
            case 2:
                n = shotSun();
                break;
            case 3:
                n = shotRush();
                break;
            case 4:
                n = shotCircle();
                break;
            case 5:
                n = shotSquare();
                break;
            case 6:
            case 7:
            case 8:
            case 9:
                n = shotSim();
                break;
            case 10:
            case 11:
            case 12:
            case 13:
            case 14:
            case 15:
                n = shotEnemy();
                break;
            default:
                //n = 0;
                shotDefault();
                if (turnTime>3f)
                {
                    if (hp>MAXHP/2)
                    {
                        n = Random.Range(0,10);
                    }else
                    {
                        n = Random.Range(0, 16);
                    }
                    turnTime = 0f;
                }
                break;
        }
        if (n == -1)
        {
            shotflag = false;
        }

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

    int shotEnemy()
    {
        if (!shotflag)
        {
            for (int lp = 0; lp < coolShotEnemy.Length; lp++) coolShotEnemy[lp] = Random.Range(0.1f, 1);
            shotflag = !shotflag;
        }
        for (int lp = -2; lp <= 2; lp++)
        {
            if (coolShotEnemy[lp + 2] < 0f && coolShotEnemy[lp + 2] > -10f)
            {
                GeneratorNenemy(bullets[(int)Random.Range(0,bullets.Length)], lp);
                coolShotEnemy[lp + 2] = -10f;
            }
            else
            {
                coolShotEnemy[lp + 2] -= Time.deltaTime;
            }
        }
        if ((coolShotEnemy[0] < -10f && coolShotEnemy[1] < -10f && coolShotEnemy[2] < -10f &&
     coolShotEnemy[3] < -10f && coolShotEnemy[4] < -10f)) return -1;
        return n;
    }//1

    int shotSun()
    {
        if (!shotflag)
        {
            for (int lp = 0; lp < coolShotEnemy.Length; lp++)
            {
                if(n%2==0) coolShotEnemy[lp] = lp * 0.2f;
                else coolShotEnemy[coolShotEnemy.Length-1-lp] = lp * 0.2f;
            }
            shotflag = !shotflag;
        }
        for (int lp = -2; lp <= 2; lp++)
        {
            if (coolShotEnemy[lp + 2] < 0f && coolShotEnemy[lp + 2] > -10f)
            {
                GeneratorNenemy(bullets[3], lp);
                coolShotEnemy[lp + 2] = -10f;
            }
            else
            {
                coolShotEnemy[lp + 2] -= Time.deltaTime;
            }
        }
        if ((coolShotEnemy[0] < -10f && coolShotEnemy[1] < -10f && coolShotEnemy[2] < -10f &&
     coolShotEnemy[3] < -10f && coolShotEnemy[4] < -10f)) return -1;
        return n;
    }//2

    int shotRush()
    {
        if (!shotflag)
        {
            for (int lp = 0; lp < coolShotEnemy.Length; lp++)
            {
                coolShotEnemy[lp] = Random.Range(0.2f, 0.8f);
            }
            shotflag = !shotflag;
        }
        for (int lp = -2; lp <= 2; lp++)
        {
            if (turnTime> coolShotEnemy[lp + 2])
            {
                coolShotEnemy[lp + 2] = turnTime + Random.Range(0.2f, 0.8f);
                GeneratorNenemy(bullets[0], lp);
            }
        }
        if ((turnTime>6f)) return -1;
        return n;
    }//1

    int shotCircle()
    {
        if (!shotflag)
        {
            for (int lp = 0; lp < coolShotEnemy.Length; lp++) coolShotEnemy[lp] = Random.Range(0.1f, 1f);
            shotflag = !shotflag;
        }
        for (int lp = -2; lp <= 2; lp++)
        {
            if (coolShotEnemy[lp + 2] < 0f && coolShotEnemy[lp + 2] > -10f)
            {
                GeneratorNenemy(bullets[7], lp);
                if(Random.Range(0f,2f)<1f) coolShotEnemy[lp + 2] = -10f;
                else coolShotEnemy[lp + 2] = Random.Range(0.2f, 0.6f);
            }
            else
            {
                coolShotEnemy[lp + 2] -= Time.deltaTime;
            }
        }
        if ((coolShotEnemy[0] < -10f && coolShotEnemy[1] < -10f && coolShotEnemy[2] < -10f &&
     coolShotEnemy[3] < -10f && coolShotEnemy[4] < -10f)) return -1;
        return n;
    }//1
    int shotSquare()
    {
        if (!shotflag)
        {
            for (int lp = 0; lp < coolShotEnemy.Length; lp++) coolShotEnemy[lp] = Random.Range(0.1f, 1f);
            shotflag = !shotflag;
        }
        for (int lp = -2; lp <= 2; lp++)
        {
            if (coolShotEnemy[lp + 2] < 0f && coolShotEnemy[lp + 2] > -10f)
            {
                GeneratorNenemy(bullets[5], lp);
                if (Random.Range(0f, 2f) < 1f) coolShotEnemy[lp + 2] = -10f;
                else coolShotEnemy[lp + 2] = Random.Range(0.2f, 0.6f);
            }
            else
            {
                coolShotEnemy[lp + 2] -= Time.deltaTime;
            }
        }
        if ((coolShotEnemy[0] < -10f && coolShotEnemy[1] < -10f && coolShotEnemy[2] < -10f &&
     coolShotEnemy[3] < -10f && coolShotEnemy[4] < -10f)) return -1;
        return n;
    }//1

    int shotSim()
    {
        if (!shotflag)
        {
            for (int lp = 0; lp < coolShotEnemy.Length; lp++) coolShotEnemy[lp] = 0.5f;
            shotflag = !shotflag;
        }
        for (int lp = -2; lp <= 2; lp++)
        {
            if (coolShotEnemy[lp + 2] < 0f && coolShotEnemy[lp + 2] > -10f)
            {
                GeneratorNenemy(bullets[n % 4], lp);
                coolShotEnemy[lp + 2] = -10f;
            }
            else
            {
                coolShotEnemy[lp + 2] -= Time.deltaTime;
            }
        }
        if ((coolShotEnemy[0] < -10f && coolShotEnemy[1] < -10f && coolShotEnemy[2] < -10f &&
     coolShotEnemy[3] < -10f && coolShotEnemy[4] < -10f)) return -1;
        return n;
    }//4

    public override void OnHit(GameObject obj, GameObject objOpp)
    {
        if (objOpp.tag == "PlayerBullet")
        {
            if (hp > 0)
            {
                Camera.main.GetComponent<gameObj>().score += 10;
                hp--;
                Destroy(objOpp);
            }else
            {
                Camera.main.GetComponent<gameObj>().score += 1000;
                outDestroy(obj);
            }
        }
    }
}
