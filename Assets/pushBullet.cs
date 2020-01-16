using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushBullet : MonoBehaviour
{
    Vector3 vect;
    gameObj objRule;
    bool attackPlayer;
    // Start is called before the first frame update
    void Start()
    {
        vect = (Camera.main.GetComponent<gameObj>().player.transform.position - transform.position).normalized
            * Camera.main.GetComponent<gameObj>().scrollSpeed * 4f ;
        objRule= objRule = Camera.main.GetComponent<gameObj>();
        attackPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackPlayer)
        {
            if (objRule.player.GetComponent<player>().onPlay)
            {
                transform.position += vect * objRule.gameDeltaTime*2f;
                var ang = transform.eulerAngles;
                ang.z += (int)objRule.gameDeltaTime * 36;
                transform.eulerAngles = ang;
            }
            else
            {
                gameObject.tag = "LostEnemyBullet";
                vect = objRule.scoreText.transform.position - transform.position;
                attackPlayer = !attackPlayer;
            }
        }
        else
        {
            var trans = transform.position + vect * objRule.gameDeltaTime * 3f;
            transform.position = trans;
        }

        if (objRule.ScreenSize[0].x>transform.position.x|| transform.position.x > objRule.ScreenSize[1].x||
            objRule.ScreenSize[0].y > transform.position.y || transform.position.y > objRule.ScreenSize[1].y)
        {
            Destroy(gameObject);
        }
    }
}
