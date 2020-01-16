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
    gameObj objRule;//ゲームルール

    public bool onPlay;//無敵時間の設定,被弾時に切り替える
    AudioSource Audio;
    [SerializeField]
    AudioClip shotSE;
    [SerializeField]
    AudioClip hitSE;
    [SerializeField]
    AudioClip reloadSE;

    public void addPlayerBullet(GameObject obj)//lineで描いた弾の装填
    {
        pStatus.bullet.Add(obj);
    }

    // Start is called before the first frame update
    void Awake()
    {
        Audio = GetComponent<AudioSource>();
        //各ステータスの初期化
        pStatus.bullet = new List<GameObject>();
        pStatus.nowBullet = nanoBullet;
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = 0;
        objRule = Camera.main.GetComponent<gameObj>();
        objRule.player = gameObject;
        onPlay = true;
        if (selectScene.playerPos.x != 0f)
        {
            transform.position = selectScene.playerPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objRule.ScreenSize[0].x<transform.position.x&& transform.position.x< objRule.ScreenSize[1].x
            && objRule.ScreenSize[0].y <= transform.position.y && transform.position.y <= objRule.ScreenSize[1].y) {
            if (onPlay) {
                shot();//発砲関連
            }else
            {
                stan();//無敵時間兼
            }
            setBullet();//弾の装填関連
            transPosition();//プレイヤー移動関連
            
            stockBullet();//line弾の視覚的整列
        }else
        {
            tformVectZero();
        }
    }
    void stan()
    {
        pStatus.coolTime -= objRule.gameDeltaTime;
        if ((pStatus.coolTime < 3f * 6f / 6 && pStatus.coolTime > 3f*5f / 6)
            || (pStatus.coolTime < 3f * 4f / 6 && pStatus.coolTime > 3f *3f/ 6)
            || (pStatus.coolTime < 3f*2f / 6 && pStatus.coolTime>3f/6))
        {
            GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.2f);
        }else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        if (pStatus.coolTime < 0f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            onPlay = !onPlay;
        }
    }

    void tformVectZero()
    {
        var pos = - transform.position.normalized;
        rig.velocity = pos * objRule.gameDeltaTime * pStatus.speed;
    }
    void setBullet()
    {
        if (Input.GetButtonDown("Reload"))
        {
            Debug.Log("Reload");
            Audio.PlayOneShot(reloadSE);
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
        float hor = Input.GetAxis("Horizontal"), ver = Input.GetAxis("Vertical");
        if (Mathf.Abs(hor)>0&&
            !(0.9f * objRule.ScreenSize[0].x < transform.position.x && transform.position.x < 0.9f * objRule.ScreenSize[1].x))
        {
            var trans = transform.position;
            if (transform.position.x < 0.9f * objRule.ScreenSize[0].x)
                trans.x = 0.9f * objRule.ScreenSize[0].x;
            else
                trans.x = 0.9f * objRule.ScreenSize[1].x;
            transform.position = trans;
            if (transform.position.x / Mathf.Abs(transform.position.x) == hor / Mathf.Abs(hor))
            {
                hor = 0;
            }
        }
        if (Mathf.Abs(ver) > 0&&
            !(0.9f * objRule.ScreenSize[0].y < transform.position.y && transform.position.y < 0.9f * objRule.ScreenSize[1].y))
        {
            var trans = transform.position;
            if (transform.position.y < 0.9f * objRule.ScreenSize[0].y)
                trans.y = 0.9f * objRule.ScreenSize[0].y;
            else
                trans.y = 0.9f * objRule.ScreenSize[1].y;
            transform.position = trans;
            if (transform.position.y / Mathf.Abs(transform.position.y) == ver / Mathf.Abs(ver))
            {
                ver = 0;
            }
        }
        rig.velocity = new Vector2(hor, ver) * objRule.gameDeltaTime * pStatus.speed;
    }

    void shot()
    {
        if (Input.GetButton("Shot"))
        {
            if (pStatus.coolTime <= 0f)//クールタイム有りの発砲
            {
                Audio.PlayOneShot(shotSE);
                var vect = transform.eulerAngles + new Vector3(0f, 0f, Random.Range(0f,0f));
                var bullet = Instantiate(pStatus.nowBullet, transform.position, Quaternion.Euler(vect));
                var inp = bullet.AddComponent<bulletImpact>();
                bullet.tag = "PlayerBullet";
                inp.speed = pStatus.speed/1000f * 8f;
                inp.vect = bullet.transform.right;
                pStatus.coolTime = pStatus.setCoolTime;
                if (pStatus.nowBullet.GetComponent<LineRenderer>() != null)//line弾を縮める
                {
                    setReSize();
                }
            }
            
        }
        if (pStatus.coolTime > 0f) pStatus.coolTime -= objRule.gameDeltaTime;
    }

    ///ここに遠隔リサイズ処理入れる

    public void reSize(LineRenderer line)
    {
        Vector2 size = line.transform.localScale;
        Vector2 bSize = line.GetComponent<bullet>().bStatus.size;
        if (size.magnitude * bSize.magnitude < 0.2f)
        {
            Debug.Log("OK" + line.name);
            for (int lp=0;lp< pStatus.bullet.Count;lp++) {
                if(line.gameObject== pStatus.bullet[lp].gameObject)
                {
                    Destroy(pStatus.bullet[lp]);
                    pStatus.bullet.Remove(pStatus.bullet[lp]);
                    if (pStatus.nowBullet== pStatus.bullet[lp]) { pStatus.nowBullet = nanoBullet; }
                    return;
                }
            }
            size *= 0.6f;
            line.transform.localScale = size;
            if (size.x < 0.01f)
            {
                Destroy(line.gameObject);
            }
        }
        else
        {
            size *= 0.8f;
            line.transform.localScale = size;
        }
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
            pos += (Vector3)difference.normalized * -2f * objRule.gameDeltaTime;
            trans.position = pos;
        }
        else//離れてるほど大きく移動させる
        {
            var pos = trans.position;
            pos += (Vector3)difference * -2f * objRule.gameDeltaTime;
            trans.position = pos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="BOSS" || collision.tag == "Enemy" || collision.tag == "EnemyBullet")
        {
            if (onPlay)
            {
                Audio.PlayOneShot(hitSE);
                Camera.main.GetComponent<gameObj>().score -= 100;
                pStatus.coolTime = 3f;
                onPlay = !onPlay;
            }
        }
    }
}