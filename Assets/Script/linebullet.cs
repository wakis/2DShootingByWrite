using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linebullet : MonoBehaviour
{
    player Player;
    [System.NonSerialized]
    public float destime=1f;
    // Start is called before the first frame update
    void Start()
    {
        Player = Camera.main.GetComponent<GAMERULE>().Player.GetComponent<player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (destime <= 1f)
        {
            destime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Camera.main.GetComponent<gameObj>().player.GetComponent<player>().onPlay)
        {
            if ((collision.tag == "Enemy" || collision.tag == "EnemyBullet" || collision.tag == "BOSS") && destime > 1f)
            {
                Debug.Log("step");
                if (collision.tag == "EnemyBullet") Destroy(collision.gameObject);
                Player.reSize(GetComponent<LineRenderer>());
            }
        }
    }
}
