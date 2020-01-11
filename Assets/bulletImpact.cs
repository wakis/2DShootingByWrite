using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletImpact : MonoBehaviour
{
    [System.NonSerialized]
    public float speed;
    [System.NonSerialized]
    public Vector3 vect;
    gameObj objRule;
    bool attackPlayer;
    public bulletImpact(float speed,Vector3 vector)
    {
        this.speed = speed;
        this.vect = vector;
    }
    // Start is called before the first frame update
    void Start()
    {
        objRule = Camera.main.GetComponent<gameObj>();
        Rigidbody2D rig;
        if (GetComponent<Rigidbody2D>() != null)
        {
            rig= GetComponent<Rigidbody2D>();
        }else
        {
            rig=gameObject.AddComponent<Rigidbody2D>();
        }
        rig.bodyType = (RigidbodyType2D)1;
        rig.velocity = speed * vect;
        attackPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackPlayer)
        {
            if (objRule.player.GetComponent<player>().onPlay)
            {
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
