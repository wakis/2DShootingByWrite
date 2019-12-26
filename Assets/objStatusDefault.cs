using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objStatusDefault : MonoBehaviour
{
    public GameObject bullet;
    gameObj objRule;
    float coolTime;
    float setCoolTime;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        objRule = Camera.main.GetComponent<gameObj>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Renewal(GameObject obj)
    {

    }

    public virtual int Setting(GameObject obj)
    {
        objRule = Camera.main.GetComponent<gameObj>();
        speed = obj.GetComponent<objStatusRenewal>().eStatus.speed;
        coolTime = -1f;
        setCoolTime = obj.GetComponent<objStatusRenewal>().eStatus.setCoolTime;
        return 1;
    }

    public virtual int Stay(GameObject obj)
    {
        Debug.Log(objRule.scrollSpeed);
        obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed);
        if(obj.transform.position.x<objRule.ScreenSize[1].x) return 1;
        return 0;
    }

    public virtual int Play(GameObject obj)
    {
        obj.transform.position = transPos_Front(obj.transform, objRule.scrollSpeed+speed);
        if (coolTime < 0f)
        {
            Instantiate(obj.GetComponent<objStatusRenewal>().bullet, obj.transform.position,
                Quaternion.Euler(new Vector3(0f, 0f, Vector3.Angle(obj.transform.position, objRule.player.transform.position))));
            coolTime = setCoolTime;
        }else
        {
            coolTime -= Time.deltaTime;
        }
        if (objRule.ScreenSize[0].x * 1.1f > obj.transform.position.x) return 1;
        return 0;
    }
    public virtual void Loss(GameObject obj)
    {
        Destroy(obj);
    }

    Vector3 transPos_Front(Transform trans,float speed)
    {
        var transPos = trans.position;
        transPos -= speed * Time.deltaTime * trans.right;
        Debug.Log(transPos);
        return transPos;
    }
}
