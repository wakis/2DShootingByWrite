using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct EnemyStatus
{
    [System.NonSerialized]
    public float coolTime;
    public int hp;
    public float setCoolTime;
    public float speed;
}
public class objStatusRenewal : MonoBehaviour
{
    public EnemyStatus eStatus;
    objStatus status;
    [System.NonSerialized]
    public objStatus nextSt;
    public GameObject bullet;
    Transform target;
    [SerializeField]
    objStatusDefault objType;
    private void Awake()
    {
        objType = (GetComponent<objStatusDefault>() != null ? GetComponent<objStatusDefault>() : new objStatusDefault());

        objType.bullet = bullet;
    }
    // Start is called before the first frame update
    void Start()
    {
        nextSt = objStatus.set;
        status = objStatus.NON;
    }

    // Update is called once per frame
    void Update()
    {

        if (nextSt != objStatus.NON)
        {
            objType.Renewal(gameObject);
            status = nextSt;
            nextSt = objStatus.NON;
            
        }
        int stepnum = 0;
        switch (status)
        {
            case objStatus.set:
                stepnum=objType.Setting(gameObject);
                break;
            case objStatus.stay:
                objType.Scroll(gameObject);
                stepnum = objType.Stay(gameObject);
                break;
            case objStatus.play:
                stepnum = objType.Play(gameObject);
                break;
            case objStatus.loss:
                objType.Loss(gameObject);
                Destroy(gameObject);
                break;
            default:
                nextSt = objStatus.set;
                break;
        }
        if (stepnum > 0) nextSt = status + 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        objType.OnHit(gameObject,collision.gameObject);
    }
}
