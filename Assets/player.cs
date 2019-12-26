using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct PlayerStatus
{
    [System.NonSerialized]
    public float coolTime;
    [System.NonSerialized]
    public GameObject nowBullet;
    [System.NonSerialized]
    public List<GameObject> bullet;

    public float setCoolTime;
    public float speed;
    public int bulletnum;

}
[RequireComponent(typeof(Rigidbody2D))]
public class player : MonoBehaviour
{
    [SerializeField]
    GameObject nanoBullet;//最小値の弾
    [SerializeField]
    PlayerStatus pStatus = new PlayerStatus();//ステータス

    Rigidbody2D rig;//移動制御用
    GAMERULE GameRule;//ゲームルール

    public void addPlayerBullet(GameObject obj)//lineで描いた弾の装填
    {
        pStatus.bullet.Add(obj);
    }

    // Start is called before the first frame update
    void Awake()
    {
        //各ステータスの初期化
        pStatus.bullet = new List<GameObject>();
        pStatus.nowBullet = nanoBullet;
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = 0;
        GameRule = Camera.main.GetComponent<GAMERULE>();
        GameRule.Player = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        setBullet();//弾の装填関連
        transPosition();//プレイヤー移動関連
        shot();//発砲関連
        stockBullet();//line弾の視覚的整列
    }
    void setBullet()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (pStatus.bullet.Count ==1&&pStatus.nowBullet == pStatus.bullet[0])
            {
                if (pStatus.nowBullet != nanoBullet) Destroy(pStatus.nowBullet.gameObject);
                pStatus.bullet.Remove(pStatus.bullet[0]);
                pStatus.nowBullet = nanoBullet;
            }
            else if (pStatus.nowBullet == nanoBullet&& pStatus.bullet.Count > 0)//line弾初期装填
            {
                pStatus.nowBullet = pStatus.bullet[0];
            }
            else if (pStatus.bullet.Count > 0)//line弾装填
            {
                Destroy(pStatus.bullet[0]);
                pStatus.bullet.Remove(pStatus.bullet[0]);
                pStatus.nowBullet = pStatus.bullet[0];
            }
            else//最小値の弾装填
            {
                if(pStatus.nowBullet != nanoBullet) Destroy(pStatus.nowBullet.gameObject);
                pStatus.nowBullet = nanoBullet;
            }
        }
        if (pStatus.bullet.Count>pStatus.bulletnum)//line弾が制限以上言った場合、古い物から破棄
        {
            Destroy(pStatus.bullet[0]);
            pStatus.bullet.Remove(pStatus.bullet[0]);
            pStatus.nowBullet = nanoBullet;
        }
    }
    void transPosition()//プレイヤーの移動
    {
        float hor = 0, ver = 0;
        if(0.9f*GameRule.ScreenSize[0].x<transform.position.x&& transform.position.x < 0.9f * GameRule.ScreenSize[1].x)
        {
            hor = Input.GetAxis("Horizontal");
        }
        else
        {
            hor = -Mathf.Abs(Input.GetAxis("Horizontal")) * transform.position.x / Mathf.Abs(transform.position.x);
        }
        if(0.9f * GameRule.ScreenSize[0].y < transform.position.y && transform.position.y < 0.9f * GameRule.ScreenSize[1].y)
        {
            ver = Input.GetAxis("Vertical");
        }
        else
        {
            ver = -Mathf.Abs(Input.GetAxis("Vertical")) * transform.position.y / Mathf.Abs(transform.position.y);
        }
        rig.velocity = new Vector2(hor, ver) * Time.deltaTime * pStatus.speed;
    }

    void shot()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (pStatus.coolTime <= 0f)//クールタイム有りの発砲
            {
                var vect = transform.eulerAngles + new Vector3(0f, 0f, Random.Range(0f,0f));
                var bullet = Instantiate(pStatus.nowBullet, transform.position, Quaternion.Euler(vect));
                var inp = bullet.AddComponent<bulletImpact>();
                inp.speed = pStatus.speed/1000f * 4f;
                inp.vect = bullet.transform.right;
                pStatus.coolTime = pStatus.setCoolTime;
                if (pStatus.nowBullet.GetComponent<LineRenderer>() != null)//line弾を縮める
                {
                    setReSize();
                }
            }
            
        }
        if (pStatus.coolTime > 0f) pStatus.coolTime -= Time.deltaTime;
    }
    void setReSize()
    {
        var line = pStatus.nowBullet.GetComponent<LineRenderer>();
        Vector2 size = line.transform.localScale;
        Vector2 bSize = line.GetComponent<bullet>().bStatus.size;
        if (size.magnitude* bSize.magnitude < 0.2f)
        {
            Destroy(pStatus.bullet[0]);
            pStatus.bullet.Remove(pStatus.bullet[0]);
            pStatus.nowBullet = nanoBullet;
        }
        else
        {
            size *= 0.8f;
            line.transform.localScale = size;
        }
    }

    void stockBullet()//弾の見た目のstock
    {
        float rootPoint = 1f;
        for (int lp=0;lp<pStatus.bullet.Count;lp++)
        {
            if (lp > 0) rootPoint += 
                    pStatus.bullet[lp - 1].GetComponent<bullet>().bStatus.size.x* pStatus.bullet[lp - 1].transform.localScale.x / 1.9f;//前のObjectとの間隔
            rootPoint += 
                pStatus.bullet[lp].GetComponent<bullet>().bStatus.size.x* pStatus.bullet[lp].transform.localScale.x / 2f;//今の弾の大きさ分の感覚
            transPositionToPlayerBack(pStatus.bullet[lp].transform,rootPoint);//移動をぬるりとさせる
        }
    }

    void transPositionToPlayerBack(Transform trans,float backPoint)
    {
        Vector2 backPosition = transform.position;
        backPosition.x -= backPoint;
        Vector2 difference = ((Vector2)trans.position - backPosition);//差異の大きさ
        if (difference.magnitude < 0.1f)//近いときの処理
        {
            return;
        }else if(difference.magnitude < 1f)//少しずつ移動させる
        {
            var pos = trans.position;
            pos += (Vector3)difference.normalized * -2f * Time.deltaTime;
            trans.position = pos;
        }
        else//離れてるほど大きく移動させる
        {
            var pos = trans.position;
            pos += (Vector3)difference * -2f * Time.deltaTime;
            trans.position = pos;
        }
    }
}