using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletImpact : MonoBehaviour
{
    [System.NonSerialized]
    public float speed;
    [System.NonSerialized]
    public Vector3 vect;
    
    public bulletImpact(float speed,Vector3 vector)
    {
        this.speed = speed;
        this.vect = vector;
    }
    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
