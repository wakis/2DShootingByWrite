using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getScore : MonoBehaviour
{
    gameObj gameRule;
    // Start is called before the first frame update
    void Start()
    {
        gameRule = Camera.main.GetComponent<gameObj>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Collider2D>().enabled== gameRule.player.GetComponent<player>().onPlay)
        {
            GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameRule.player.GetComponent<player>().onPlay)
        {
            if (collision.tag == "BOSS" || collision.tag == "Enemy" || collision.tag == "EnemyBullet")
            {
                gameRule.score -= 10;
            }else if (collision.tag == "PlayerBullet")
            {
                gameRule.score += 1;
            }
        }
    }
}
