using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stranightBullet : MonoBehaviour
{
    gameObj objRule;
    bool attackPlayer;
    Vector3 vect;
    // Start is called before the first frame update
    void Start()
    {
        objRule = objRule = Camera.main.GetComponent<gameObj>();
        attackPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackPlayer)
        {
            if (objRule.player.GetComponent<player>().onPlay)
            {
                transform.position -= Vector3.right * Time.deltaTime * Camera.main.GetComponent<gameObj>().scrollSpeed * 4f;
            }
            else
            {
                vect = objRule.scoreText.transform.position - transform.position;
                attackPlayer = !attackPlayer;
            }
        }
        else
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
