using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBullet : MonoBehaviour
{
    Vector3 vect;
    gameObj objRule;
    bool front;
    bool attackPlayer;
    // Start is called before the first frame update
    void Start()
    {
        vect = Vector3.right * 2f;
        objRule = objRule = Camera.main.GetComponent<gameObj>();
        front = false;
        attackPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackPlayer)
        {
            if (objRule.player.GetComponent<player>().onPlay)
            {
                if (!front)
                {
                    vect = ((Camera.main.GetComponent<gameObj>().player.transform.position - transform.position).normalized - vect) / 2f;
                    if (transform.position.x < objRule.player.transform.position.x) front = !front;
                }
                else
                {
                    if (transform.position.x > objRule.player.transform.position.x + 1f) front = !front;
                }
                transform.position += vect * Time.deltaTime * Camera.main.GetComponent<gameObj>().scrollSpeed * 4f;
                var ang = transform.eulerAngles;
                ang.z += (int)(Time.deltaTime * 720f);
                transform.eulerAngles = ang;
            }
            else
            {
                vect = objRule.scoreText.transform.position - transform.position;
                attackPlayer = !attackPlayer;
            }
        }else
        {
            var trans = transform.position + vect * Time.deltaTime * 3f;
            transform.position = trans;
        }
        if (objRule.ScreenSize[0].x > transform.position.x || transform.position.x > objRule.ScreenSize[1].x ||
            objRule.ScreenSize[0].y > transform.position.y || transform.position.y > objRule.ScreenSize[1].y)
        {
            Destroy(gameObject);
        }
    }
    
}
