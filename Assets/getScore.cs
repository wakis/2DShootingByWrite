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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameRule.player.GetComponent<player>().onPlay)
        {
            if (collision.tag == "LostEnemyBullet")
            {
                gameRule.score -= 10;
            }else if (collision.tag == "LostPlayerBullet")
            {
                gameRule.score += 1;
            }
        }
    }
}
