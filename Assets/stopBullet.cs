using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopBullet : MonoBehaviour
{
    gameObj objRule;
    bool stay;
    Vector3 vect;
    float shotTime;
    bool attackPlayer;
    // Start is called before the first frame update
    void Start()
    {
        objRule = objRule = Camera.main.GetComponent<gameObj>();
        stay = false;
        shotTime = 0f;
        attackPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackPlayer)
        {
            if (objRule.player.GetComponent<player>().onPlay)
            {
                if (!stay)
                {
                    transform.position -= Vector3.right * objRule.gameDeltaTime * Camera.main.GetComponent<gameObj>().scrollSpeed * 4f;
                    if (-(objRule.ScreenSize[1].x - objRule.ScreenSize[0].x) * 0.4f > transform.position.x) stay = !stay;
                }
                else
                {
                    if (shotTime < 1f)
                    {
                        shotTime += objRule.gameDeltaTime;
                        reVect();
                    }
                    else
                    {
                        transform.position += vect * objRule.gameDeltaTime * Camera.main.GetComponent<gameObj>().scrollSpeed * 10f;
                    }
                }
                var ang = transform.eulerAngles;
                ang.z += (int)(objRule.gameDeltaTime * 720f);
                transform.eulerAngles = ang;
            }
            else
            {
                gameObject.tag = "LostEnemyBullet";
                vect = objRule.scoreText.transform.position - transform.position;
                attackPlayer = !attackPlayer;
            }
        }else
        {
            var trans = transform.position + vect * objRule.gameDeltaTime*3f;
            transform.position = trans;
        }
        if (objRule.ScreenSize[0].x > transform.position.x || transform.position.x > objRule.ScreenSize[1].x ||
                    objRule.ScreenSize[0].y > transform.position.y || transform.position.y > objRule.ScreenSize[1].y)
        {
            Destroy(gameObject);
        }
    }
    void reVect()
    {
        vect = (Camera.main.GetComponent<gameObj>().player.transform.position - transform.position).normalized;
    }
}
