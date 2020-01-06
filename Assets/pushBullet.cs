using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushBullet : MonoBehaviour
{
    Vector3 vect;
    gameObj objRule;
    // Start is called before the first frame update
    void Start()
    {
        vect = (Camera.main.GetComponent<gameObj>().player.transform.position - transform.position).normalized
            * Camera.main.GetComponent<gameObj>().scrollSpeed * 4f ;
        objRule= objRule = Camera.main.GetComponent<gameObj>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += vect * Time.deltaTime;
        var ang = transform.eulerAngles;
        ang.z += (int)Time.deltaTime*36;
        transform.eulerAngles = ang;
        if (objRule.ScreenSize[0].x>transform.position.x|| transform.position.x > objRule.ScreenSize[1].x||
            objRule.ScreenSize[0].y > transform.position.y || transform.position.y > objRule.ScreenSize[1].y)
        {
            Destroy(gameObject);
        }
    }
}
