using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stranightBullet : MonoBehaviour
{
    gameObj objRule;
    // Start is called before the first frame update
    void Start()
    {
        objRule = objRule = Camera.main.GetComponent<gameObj>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= Vector3.right * Time.deltaTime * Camera.main.GetComponent<gameObj>().scrollSpeed * 4f;
        if (objRule.ScreenSize[0].x > transform.position.x || transform.position.x > objRule.ScreenSize[1].x ||
            objRule.ScreenSize[0].y > transform.position.y || transform.position.y > objRule.ScreenSize[1].y)
        {
            Destroy(gameObject);
        }
    }
}
