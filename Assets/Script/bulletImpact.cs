﻿using System.Collections;
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
    bool nano;
    Rigidbody2D rig;
    public bulletImpact(float speed,Vector3 vector)
    {
        this.speed = speed;
        this.vect = vector;
    }
    // Start is called before the first frame update
    void Start()
    {
        objRule = Camera.main.GetComponent<gameObj>();
        if (GetComponent<Rigidbody2D>() != null)
        {
            rig= GetComponent<Rigidbody2D>();
        }else
        {
            rig=gameObject.AddComponent<Rigidbody2D>();
        }
        rig.bodyType = (RigidbodyType2D)1;
        rig.velocity = speed * vect * objRule.gradationTimePer;
        attackPlayer = true;
        if (GetComponent<LineRenderer>())
        {
            nano = false;
        }else
        {
            nano = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rig.velocity = speed * vect * objRule.gradationTimePer;
        if (attackPlayer)
        {
            if (objRule.player.GetComponent<player>().onPlay)
            {
            }
            else
            {
                gameObject.tag = "LostPlayerBullet";
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                vect = objRule.scoreText.transform.position - transform.position;
                attackPlayer = !attackPlayer;
            }
        }
        else
        {
            var trans = transform.position + vect * objRule.gameDeltaTime * 10f;
            transform.position = trans;
        }
        if (objRule.ScreenSize[0].x > transform.position.x || transform.position.x > objRule.ScreenSize[1].x ||
            objRule.ScreenSize[0].y > transform.position.y || transform.position.y > objRule.ScreenSize[1].y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (nano&&(collision.tag == "BOSS" || collision.tag == "Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
