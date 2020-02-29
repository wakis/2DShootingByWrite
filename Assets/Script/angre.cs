using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angre : MonoBehaviour
{
    public Transform[] obj = new Transform[2];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2[] vect =
        {
            obj[0].position-transform.position,
            obj[1].position-transform.position
        };
        Debug.Log(Vector2.Angle(vect[0], vect[1]));
    }
}
