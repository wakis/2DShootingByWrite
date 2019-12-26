using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushBullet : MonoBehaviour
{
    Vector3 vect;
    // Start is called before the first frame update
    void Start()
    {
        vect = (Camera.main.GetComponent<gameObj>().player.transform.position - transform.position).normalized
            * Camera.main.GetComponent<gameObj>().scrollSpeed * 4f ;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += vect * Time.deltaTime;
    }
}
