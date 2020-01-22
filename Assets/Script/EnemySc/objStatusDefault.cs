using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objStatusDefault : MonoBehaviour
{
    public GameObject bullet;
    protected　gameObj objRule;
    protected float coolTime;
    protected float setCoolTime;
    protected float speed;
    // Start is called before the first frame update
    void Start()
    {
        objRule = Camera.main.GetComponent<gameObj>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Scroll(GameObject obj)
    {

        obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
    }

    public virtual void Renewal(GameObject obj)//切り替え時
    {

    }

    public virtual int Setting(GameObject obj)//ゲーム開始時
    {
        objRule = Camera.main.GetComponent<gameObj>();
        speed = obj.GetComponent<objStatusRenewal>().eStatus.speed;
        coolTime = -1f;
        setCoolTime = obj.GetComponent<objStatusRenewal>().eStatus.setCoolTime;
        return 1;
    }

    public virtual int Stay(GameObject obj)//待機状態
    {

        obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
        if (obj.transform.position.x<objRule.ScreenSize[1].x) return 1;
        return 0;
    }

    public virtual int Play(GameObject obj)//画面内行動
    {
        obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed+speed);
        if (coolTime < 0f)
        {
            Instantiate(obj.GetComponent<objStatusRenewal>().bullet, obj.transform.position,
                Quaternion.Euler(new Vector3(0f, 0f, Vector3.Angle(obj.transform.position, objRule.player.transform.position))));
            coolTime = setCoolTime;
        }else
        {
            coolTime -= objRule.gameDeltaTime;
        }
        if (objRule.ScreenSize[0].x * 1.1f > obj.transform.position.x) return 1;
        return 0;
    }
    public virtual void Loss(GameObject obj)//消失時
    {
        
    }

    protected Vector3 transPos_Front(Transform trans,float speed)
    {
        var transPos = trans.position;
        transPos -= speed * objRule.gameDeltaTime * trans.right;

        return transPos;
    }

    public virtual void OnHit(GameObject obj, GameObject objOpp ,GameObject[] bomEffect)
    {
        if (objOpp.tag == "PlayerBullet")
        {
            var desobj = Instantiate(bomEffect[(int)Random.Range(0, bomEffect.Length)], transform.position, Quaternion.Euler(transform.eulerAngles));
            desobj.transform.localScale = transform.localScale*2f;
            outDestroy(obj);
        }
    }

    protected virtual void outDestroy(GameObject obj)
    {
        Camera.main.GetComponent<gameObj>().score += 20;
        Destroy(obj);
    }
}
