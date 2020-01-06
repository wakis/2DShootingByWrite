using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameObj : MonoBehaviour
{
    public float scrollSpeed = 1f;
    public Vector3[] ScreenSize = new Vector3[2];
    
    public GameObject player;
    private void Awake()
    {
        foreach (var p_obj in FindObjectsOfType<player>())
        {
            if (p_obj.tag == "Player")
            {
                player = p_obj.gameObject;
            }
        }

    }
    private void Start()
    {
        ScreenSize[0] = Camera.main.ViewportToWorldPoint(new Vector3(0, 0,10));
        ScreenSize[1] = Camera.main.ViewportToWorldPoint(new Vector3(1, 1,10));

        /* foreach (var p_obj in FindObjectsOfType<player>())
         {
             if (p_obj.tag == "Player")
             {
                 player = p_obj.gameObject;
             }
         }*/
    }
    private void Update()
    {
        if (player == null|| player.tag!="Player")
        {
            Awake();   
        }
    }
}
